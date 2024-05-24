//(function (Handsontable) {
//    var ImageRenderer = function (instance, TD, row, col, prop, value, cellProperties) {

//        Handsontable.renderers.BaseRenderer.apply(this, arguments);
//        var tmp_img = $(TD).find("img.img_" + prop);
//        if (tmp_img.length > 0) {
//            tmp_img.attr("src", value);
//            return TD;
//        }
//        $(TD).empty().css("text-align", "center");
//        if (instance.rootElement.img_prepare == null) {
//            instance.rootElement.img_prepare = $('<img></img>');
//        }
//        var tmp_img = instance.rootElement.img_prepare.clone();
//        tmp_img.addClass("img_" + prop);
//        var column_readOnly = true;
//        tmp_img.attr("style", "max-height: 150px; max-width: 200px; cursor: pointer");
//        tmp_img.attr("src", "http://localhost:23654/Upload/Temp/Banner/637001705101646446.jpg");
//        //tmp_img.img({
//        //    maxWidth: 200,
//        //    maxHeight: 150,
//        //    text: false,
//        //});
        
//        tmp_img.appendTo(TD);
//        return TD;
//    };
//    Handsontable.renderers.ImageRenderer = ImageRenderer;

//    var ImageEditor = Handsontable.editors.BaseEditor.prototype.extend();
//    ImageEditor.prototype.getValue = function () {
//        return "" + $(this.TD).find("img").val();
//    };
//    ImageEditor.prototype.setValue = function (newValue) {
//        var TD = this.TD;
//        $(TD).find("img").val(newValue);
//    };
//    ImageEditor.prototype.open = function () { };
//    ImageEditor.prototype.close = function () { };
//    ImageEditor.prototype.focus = function () {
//        var TD = this.TD;
//        if (!$(TD).find("img").is(":focus")) {
//            $(TD).find("img").trigger("focus");
//        }
//    };
//    ImageEditor.prototype.beginEditing = function (initialValue, event) {
//        this.focus();
//        return;
//    };
//    ImageEditor.prototype.finishEditing = function (revertToOriginal, ctrlDown, callback) {
//        return;
//    };
//    Handsontable.editors.ImageEditor = ImageEditor;

//    Handsontable.ImageCell = {
//        editor: Handsontable.editors.ImageEditor,
//        renderer: Handsontable.renderers.ImageRenderer
//    };
//    $.extend(Handsontable.cellTypes, {
//        img: Handsontable.ImageCell
//    });
//})(Handsontable);
//20190722 volcan add image renderer
(function (Handsontable) {
    var ImageRenderer = function (instance, TD, row, col, prop, value, cellProperties) {
        var escaped = Handsontable.helper.stringify(value), imag;

        if (escaped.indexOf('http') === 0) {
            imag = document.createElement('IMG');
            //imag.style.cursor = "pointer";
            imag.style.maxHeight = "150px";
            imag.style.maxWidth = "200px";

            if (value == null || value == "") {
                //imag.src = "../../Content/images/icon/icons8-upload-32.png";
            }
            else imag.src = value;

            Handsontable.dom.addEvent(imag, 'mousedown', function (e) {
                e.preventDefault(); // prevent selection quirk
            });

            Handsontable.dom.empty(TD);
            TD.appendChild(imag);
            TD.style.textAlign = 'center';
            TD.style.height = "150px";
            TD.style.width = "200px";
        }
        else {
            // render as text
            Handsontable.renderers.TextRenderer.apply(this, arguments);
        }
        return TD;
    };
    Handsontable.renderers.ImageRenderer = ImageRenderer;

    var ImageEditor = Handsontable.editors.BaseEditor.prototype.extend();
    ImageEditor.prototype.getValue = function () { };
    ImageEditor.prototype.setValue = function (newValue) { };
    ImageEditor.prototype.open = function () { };
    ImageEditor.prototype.close = function () { };
    ImageEditor.prototype.focus = function () {
        var TD = this.TD;
        if (!$(TD).find("img").is(":focus")) {
            $(TD).find("img").trigger("focus");
        }
    };
    ImageEditor.prototype.beginEditing = function (initialValue, event) {
        this.focus();
        return;
    };
    ImageEditor.prototype.finishEditing = function (revertToOriginal, ctrlDown, callback) {
        return;
    };
    Handsontable.editors.ImageEditor = ImageEditor;

    Handsontable.ImageCell = {
        editor: Handsontable.editors.ImageEditor,
        renderer: Handsontable.renderers.ImageRenderer
    };
    $.extend(Handsontable.cellTypes, {
        img: Handsontable.ImageCell
    });
})(Handsontable);
(function (Handsontable) {
    var FormSetTextEditor = Handsontable.editors.TextEditor.prototype.extend();
    FormSetTextEditor.prototype.createElements = function () {
        Handsontable.editors.TextEditor.prototype.createElements.apply(this, arguments);
        if (!this.$TEXTAREA_BAK) {
            this.$TEXTAREA_BAK = $(this.TEXTAREA).clone(true);
        }
    };
    FormSetTextEditor.prototype.finishEditing = function (revertToOriginal, ctrlDown, callback) {
        Handsontable.editors.TextEditor.prototype.finishEditing.apply(this, arguments);
        $(this.TEXTAREA_PARENT).empty();
        this.TEXTAREA = this.$TEXTAREA_BAK.clone(true).appendTo(this.TEXTAREA_PARENT).get(0);
    };
    FormSetTextEditor.prototype.prepare = function (row, col, prop, td, originalValue, cellProperties) {
        Handsontable.editors.TextEditor.prototype.prepare.apply(this, arguments);
        var settings = this.instance.getSettings().columns[col].vs_initialForm;
        var allow_settings = ['M', 'N', 'NQ', 'DP', 'DC', 'EN', 'TA'];
        var formObj = this.TEXTAREA;
        $.each(allow_settings, function (idx, elm) {
            $(formObj).off(".vs_initialForm_" + elm);
        });
        $.each(settings, function (tmp_prop, tmp_val) {
            if ($.inArray(tmp_prop, allow_settings) == -1) {
                delete settings[tmp_prop];
            }
        });
        $(formObj).vs_initialForm(settings);
    };
    Handsontable.editors.FormSetTextEditor = FormSetTextEditor;
    Handsontable.FormSetTextCell = {
        editor: Handsontable.editors.FormSetTextEditor,
        renderer: Handsontable.renderers.TextRenderer
    };
    $.extend(Handsontable.cellTypes, {
        text_ini_form: Handsontable.FormSetTextCell
    });
})(Handsontable);
(function (Handsontable) {
    var SelectRenderer = function (instance, TD, row, col, prop, value, cellProperties) {
        Handsontable.renderers.BaseRenderer.apply(this, arguments);
        if (instance.rootElement.empty_select == null) {
            instance.rootElement.empty_select = $('<select class="form-select"><option></option></select>');
        }
        if (instance.rootElement.select_prepare2 == null) {
            instance.rootElement.select_prepare2 = {};
        }
        var cat_object = {};
        if (typeof (SelectCat) != "undefined") {
            cat_object = $.extend({}, SelectCat || {});
        }
        var parent_prop = prop;
        if (cat_object[prop]) {
            parent_prop = cat_object[prop];
        }
        var tmp_select = $(TD).find("select.sel_" + prop);
        if (tmp_select.length > 0) {//re-render
            //console.log("re-render:" + row + ":" + col);
            if (parent_prop != prop) {
                var parent_value = instance.getSourceDataAtRow(row)[parent_prop];
                //console.log(parent_value)
                var new_select = null;
                if (parent_value != null && parent_value!="") {
                    new_select = instance.rootElement.select_prepare2[prop][parent_value].clone();
                } else {
                    new_select = instance.rootElement.empty_select.clone();
                }
                //console.log(new_select.get(0).options.length)
                tmp_select.empty().html(new_select.html())
            }

            tmp_select.val(value);
            if (tmp_select.val() != value) {
                tmp_select.val("");
            }
            return TD;
        }
        
        $(TD).empty().css("text-align", "left");
        //console.log(instance.getCoords(instance.getCell(row, col, true)));
        //instance.getSettings().columns[col].readOnly = true;//cell readOnly//In Renderer runtime, instance.getCoords(TD).row != row
        //console.log(instance.getSettings().columns[col].type);

        if (instance.rootElement.select_prepare2[prop]==null) {
            var parent_Data = null;
            //var parent_cat = instance.getDataAtRowProp(row, parent_prop);
            //parent_Data = HtSelectData[parent_prop];
            if (parent_prop == prop) {
                parent_Data = [{ "Code": "0", "Name": "parent_elem" }];
            } else {
                parent_Data = HtSelectData[parent_prop];
            }
            var Data = HtSelectData[prop];
            if (Data == null) {
                Data = [];
            }
            var tmp_dom_obj = {};
            $.each(parent_Data, function (parent_idx, parent_elem) {
                var tmp_dom = instance.rootElement.empty_select.clone().get(0);
                var tmp_code = parent_elem.Code;
                var tmp_counter = 0;
                $.each(Data, function (idx, elem) {
                    if (tmp_code == "0" || elem.Cat == tmp_code) {
                        tmp_dom.options[tmp_counter - 0 + 1] = new Option(elem["Code"]+' '+elem["Name"], elem["Code"]);//Name,Code
                        tmp_counter++;
                    }
                });
                tmp_dom_obj[tmp_code] = $(tmp_dom);
            });
            instance.rootElement.select_prepare2[prop] = tmp_dom_obj;
        }
        //console.log(instance.rootElement.select_prepare2[prop]);
        //instance.getSourceDataAtRow(row)
        if (parent_prop==prop) {
            tmp_select = instance.rootElement.select_prepare2[prop]["0"].clone();
        } else {
            //console.log("BBBB:" + parent_prop);
            var parent_value = instance.getSourceDataAtRow(row)[parent_prop];
            if (parent_value != null && parent_value != "") {
                tmp_select = instance.rootElement.select_prepare2[prop][parent_value].clone();
            } else {
                tmp_select = instance.rootElement.empty_select.clone();
            }
        }
        tmp_select.val(value);
        if (tmp_select.val() != value) {
            tmp_select.val("");
        }
        /*if (instance.rootElement.select_prepare == null) {
            var Data = HtSelectData[prop];
            var tmp_dom = $('<select><option></option></select>').get(0);
            $.each(Data, function (idx, elem) {
                tmp_dom.options[idx - 0 + 1] = new Option(elem["Name"], elem["Code"]);//Name,Code
            });

            instance.rootElement.select_prepare = $(tmp_dom);
        }
        tmp_select = instance.rootElement.select_prepare.clone();
        tmp_select.val(value);
        if (tmp_select.val() != value) {
            tmp_select.val("");
        }*/
        /*$.each(tmp_select.get(0).options, function (idx, elem) {
            if (elem.value == value) {
                elem.selected = true;
                elem.defaultSelected = true;
                return false;
            }
        });*/
        
        /*if (instance.getSettings().readOnly) {
            tmp_select.attr("disabled", true);
            tmp_select.addClass("bgReadOnly");
        }*/

        //disabled setting
        var settings = null;
        try { settings = instance.getSettings().columns[col].vs_initialForm; } catch (ignore) { }
        var column_readOnly = false;
        try { column_readOnly = instance.getSettings().columns[col].readOnly; } catch (ignore) { }
        if (instance.getSettings().readOnly || column_readOnly) {
            settings = $.extend({},settings,{"D":1});
        }
        if ($.isPlainObject(settings)) {
            var allow_settings = ['D'];
            $.each(allow_settings, function (idx, elm) {
                $(tmp_select).off(".vs_initialForm_" + elm);
            });
            $.each(settings, function (tmp_prop, tmp_val) {
                if ($.inArray(tmp_prop, allow_settings) == -1) {
                    delete settings[tmp_prop];
                }
            });
            $(tmp_select).vs_initialForm(settings);
        }
        tmp_select.addClass("sel_" + prop);//.css("width", "100%")
        tmp_select.on("mousedown", function (event) {
            //console.log("mousedown 1");
            event.stopPropagation();
            var tmp_arr = instance.getSelected();
            var tmp_r = instance.getCoords(TD).row;
            var tmp_c = instance.getCoords(TD).col;
            var tmp_editor = instance.getActiveEditor();
            if (!tmp_editor || tmp_editor.TD != TD || tmp_arr == null || tmp_arr[0] != tmp_r || tmp_arr[2] != tmp_r || tmp_arr[1] != tmp_c || tmp_arr[3] != tmp_c) {
                if (tmp_editor && tmp_editor.TD && tmp_editor.TD != TD) {
                    $(tmp_editor.TD).find("select").trigger("blur");
                }
                instance.selectCellByProp(tmp_r, prop, tmp_r, prop, false);
                tmp_editor = instance.getActiveEditor();
            }
            if (tmp_editor && tmp_editor.TD == TD && !tmp_editor.isOpened()) {
                tmp_editor.beginEditing(tmp_select.val());
            }
            //console.log("mousedown 2");
        }).on("keydown", function (event) {
            //console.log("keydown" + String.fromCharCode(event.which));
            var tmp_stop = false;
            //var tmp_arr = instance.getSelected();
            var tmp_r = instance.getCoords(TD).row;
            var tmp_c = instance.getCoords(TD).col;
            var tmp_editor = instance.getActiveEditor();
            switch (event.which) {
                case 37://left
                case 38://up
                case 39://right
                case 40://down
                    if (!$.browser.mozilla) {//firefox no need to modify event
                        if (event.which == 37 || event.which == 38) {
                            if (this.selectedIndex > 0) {
                                this.selectedIndex--;
                            }
                        } else if (event.which == 39 || event.which == 40) {
                            if (this.selectedIndex < this.length - 1) {
                                this.selectedIndex++;
                            }
                        }
                        event.preventDefault();
                    }
                    tmp_stop = true;
                    break;
                case 13://enter
                    if (tmp_editor && tmp_editor.TD == TD) {
                        tmp_stop = true;
                        if (tmp_editor.isOpened()) {
                            tmp_select.trigger("blur");
                            if (tmp_r + 1 < instance.countRows()) {
                                tmp_r = tmp_r + 1;
                            }
                        } else {
                            tmp_editor.setValue(tmp_select.val());
                        }
                    } else {
                        instance.setDataAtRowProp(tmp_r, prop, tmp_select.val());
                    }
                    instance.selectCellByProp(tmp_r, prop, tmp_r, prop, false);//cause finishEditing
                    break;
                case 9://tab
                case 27://esc
                    tmp_select.trigger("blur");
                    instance.selectCellByProp(tmp_r, prop, tmp_r, prop, false);//cause finishEditing
                    break;
                default:
                    tmp_stop = true;
            }
            if (tmp_stop) {
                event.stopImmediatePropagation();
            }
        }).on("change", function (event) {
            //console.log("change");
            var tmp_r = instance.getCoords(TD).row;
            var tmp_editor = instance.getActiveEditor();
            if (tmp_editor && tmp_editor.TD == TD) {
                if (tmp_editor.isOpened()) {
                    tmp_select.trigger("blur");
                } else {
                    tmp_editor.setValue(tmp_select.val());
                }
            } else {
                instance.setDataAtRowProp(tmp_r, prop, tmp_select.val());
            }
            /*$.each(cat_object, function (idx, elem) {
                if (prop == elem) {
                    var child_prop = idx;
                    console.log(idx);
                }
            });*/
            instance.selectCellByProp(tmp_r, prop, tmp_r, prop, false);//cause finishEditing
        });
        tmp_select.appendTo(TD);
        return TD;
    };
    Handsontable.renderers.SelectRenderer = SelectRenderer;

    var SelectEditor = Handsontable.editors.BaseEditor.prototype.extend();
    /*SelectEditor.prototype.init = function () {
        console.log("init");
        Handsontable.editors.BaseEditor.prototype.init.apply(this, arguments);
        this.isCellEdited = false;
    };
    SelectEditor.prototype.prepare = function (row, col, prop, TD, originalValue, cellProperties) {
        console.log("prepare");
        Handsontable.editors.BaseEditor.prototype.prepare.apply(this, arguments);
    };*/
    SelectEditor.prototype.getValue = function () {
        //console.log("getValue");
        return "" + $(this.TD).find("select").val();
    };
    SelectEditor.prototype.setValue = function (newValue) {
        //console.log("setValue value=" + newValue);
        var TD=this.TD;
        $(TD).find("select").val(newValue);
        if ($(TD).find("select").val() != newValue) {
            $(TD).find("select").val("");
        }
    };
    SelectEditor.prototype.open = function () { };
    SelectEditor.prototype.close = function () { };
    SelectEditor.prototype.focus = function () {
        //console.log("focus 1");
        var TD = this.TD;
        //console.log($(TD).find("select").length);
        if (!$(TD).find("select").is(":focus")) {
            $(TD).find("select").trigger("focus");
        }
        $(TD).find("select").css("background-color", "#ffef8f");
        //Handsontable.editors.BaseEditor.prototype.focus.apply(this, arguments);
        //console.log("focus 2");
    };
    SelectEditor.prototype.beginEditing = function (initialValue, event) {
        //console.log("beginEditing 1");
        var TD = this.TD;
        initialValue = $(TD).find("select").val();
        Handsontable.editors.BaseEditor.prototype.beginEditing.apply(this, arguments);
        //console.log("beginEditing 2");
    };
    SelectEditor.prototype.finishEditing = function (revertToOriginal, ctrlDown, callback) {
        //console.log("finishEditing 1");
        var TD = this.TD;
        var old_value = this.getValue();
        var new_value = $(TD).find("select").val();
        if (old_value != new_value) {
            this.setValue(new_value);
        }
        $(TD).find("select").css("background-color", "");
        Handsontable.editors.BaseEditor.prototype.finishEditing.apply(this, arguments);
        //console.log("finishEditing 2");
    };
    /*SelectEditor.prototype.discardEditor = function (validationResult) {
        console.log("discardEditor 1");
        Handsontable.editors.BaseEditor.prototype.discardEditor.apply(this, arguments);
        console.log("discardEditor 2");
    };
    SelectEditor.prototype.saveValue = function (validationResult) {
        console.log("saveValue");
        Handsontable.editors.BaseEditor.prototype.saveValue.apply(this, arguments);
    };*/

    Handsontable.editors.SelectEditor = SelectEditor;

    Handsontable.SelectCell = {
        editor: Handsontable.editors.SelectEditor,
        renderer: Handsontable.renderers.SelectRenderer
    };
    $.extend(Handsontable.cellTypes, {
        select: Handsontable.SelectCell
    });
})(Handsontable);
$(document).ready(function () {
    $("div.handsontable.ht_master").parent("div.handsontable[id]").each(function (idx, elem) {
        //console.log($(elem).prop("id"));
        var tmp_instance = null;
        try { tmp_instance = $(elem).handsontable('getInstance'); } catch (ignoreerr) { }
        if (tmp_instance == null) { return; }
        //console.log(Handsontable);
        //console.log(tmp_instance.countRows());
        //console.log($.extend({}, tmp_instance.getSettings()));
        tmp_instance.addHook("afterRenderer", function (TD, row, col, prop, value, cellProperties) {
            var instance = this;
            $.each(["BO", "BC"], function (idx, arr_val) {
                var tmp_col_idx = instance.propToCol(arr_val);
                if (typeof tmp_col_idx == "number") {
                    var checked = false;
                    checked = (instance.getDataAtCell(row, tmp_col_idx) == "true");
                    //try { checked = !!$.parseJSON(instance.getDataAtCell(row, tmp_col_idx)); } catch (ignore) { }
                    if (checked) {
                        $(TD).addClass("ht_selected_row");
                    } else {
                        $(TD).removeClass("ht_selected_row");
                    }
                    return false;
                }
            });
        });
        /*tmp_instance.addHook("afterRender", function (isForced) {
            var instance = this;
            //setColor_onTD
            var tmp_arr = $.extend([], instance.getDataAtProp("BC"), instance.getDataAtProp("BO"));
            if (tmp_arr.length > 0) {
                $(instance.rootElement).find("tr:has(td)").each(function (idx, elem) {
                    var tmp_r = instance.getCoords($(elem).find("td:first").get(0)).row;
                    var checked = false;
                    if (!!tmp_arr[tmp_r]) {
                        try { checked = !!$.parseJSON(tmp_arr[tmp_r]); } catch (ignore) { }
                    }
                    if (checked) {
                        $(elem).find("td").addClass("ht_selected_row");
                    } else {
                        $(elem).find("td").removeClass("ht_selected_row");
                    }
                });
            }
        });*/
        /*tmp_instance.addHook("beforeRender", function (isForced) {
            isForced = true;
            window.tmp_timestamp1 = (new Date()).getTime();
        });
        tmp_instance.addHook("afterRender", function (isForced) {
            isForced = true;
            window.tmp_timestamp2 = (new Date()).getTime();
            console.log(window.tmp_timestamp2 - window.tmp_timestamp1);
        });*/
        tmp_instance.addHook("afterSelectionEnd", function (row, col, row2, col2) {
            //console.log("afterSelectionEnd");
            var instance = this;
            var container = instance.rootElement;
            if (row >= 0 && col >= 0 && row == row2 && col == col2 && instance.getSettings().data.length > 0) {
                if ($(container).find("button:focus").length > 0) {
                    $(container).find("button:focus").trigger("blur");
                }
            }
        });
        tmp_instance.addHook("afterValidate", function (isValid, value, row, prop) {
            var instance = this;
            var container = instance.rootElement;
            var col = instance.propToCol(prop);
            instance.setCellMeta(row, col, 'valid', isValid);
        });
        /*if (tmp_instance.getSettings().renderAllRows == null) {
            tmp_instance.updateSettings({ renderAllRows: true });
            var tmp_r = tmp_instance.countRows();
            var tmp_c = tmp_instance.countCols();
            tmp_instance.selectCell(tmp_r, tmp_c, tmp_r, tmp_c, true);
            tmp_instance.render();
            tmp_instance.deselectCell();
        }*/
        /*tmp_instance.updateSettings({
            colHeaders: function (col) {
                var tmp_obj = $.extend({}, tmp_instance.getSettings().columns);
                console.log(tmp_obj);
                tmp_instance.getSettings().columns[col].header_prop = "ZZ";
                return tmp_instance.getSettings().columns[col].header;
            }
        });*/
        tmp_instance.updateSettings({
            afterGetColHeader: function (col, TH) {//select_all button "BC",select_all button multi_chk,add row button "BA"
                //console.log("afterGetColHeader" + tmp_instance.getCoords(TH).col + "--" + col);
                //var container = this.rootElement;
                var instance = this;
                var column_setting = null;
                try { column_setting = instance.getSettings().columns[col]; } catch (ignore) { }
                if (col >= 0 && instance.getCoords(TH).col == col && column_setting != null) {//instance.countRows <> instance.getSettings().data.length
                    //console.log("afterGetColHeader" + tmp_instance.getCoords(TH).col + "--" + col);
                    var tmp_colHeader = $(TH).find("span.colHeader");
                    var tmp_function = null;
                    try { tmp_function = column_setting.click_header_function; } catch (ignore) { }
                    if (instance.getSettings().data.length > 0 && !!column_setting.select_all == true) {
                        if (column_setting.type == "multi_chk") {
                            var is_htRight = false;
                            var tmp_className = "";
                            try { tmp_className = column_setting.className; } catch (ignore) { }
                            if (tmp_className!= null && tmp_className.match(/right/ig) != null) {
                                is_htRight = true;
                            }
                            $(TH).addClass(is_htRight ? "htRight" : "htLeft");
                            var is_select_all = false;

                            var tmp_show = false;
                            var cell_readOnly = null;
                            try { cell_readOnly = column_setting.vs_initialForm.D; } catch (ignore) { }
                            var column_readOnly = false;
                            try { column_readOnly = column_setting.readOnly; } catch (ignore) { }
                            if (instance.getSettings().readOnly || column_readOnly || cell_readOnly) {
                                tmp_show = false;
                            }else{
                                //for (var tmp_r = 0; tmp_r < instance.countRows() ; tmp_r++) {
                                //}
                                tmp_show = true;
                            }
                            if (column_setting.header_button != null) {
                                var tmp_btn = column_setting.header_button;
                                is_select_all = !!column_setting.header_selected;
                                if (tmp_show) {
                                    //tmp_colHeader.empty();
                                    is_htRight?tmp_colHeader.append(tmp_btn):tmp_colHeader.prepend(tmp_btn);
                                    if (is_select_all) {
                                        tmp_btn.button("option", { icons: { primary: 'ui-icon-check' } }).addClass("ui-state-highlight");//.addClass("ui-state-highlight")
                                    } else {
                                        tmp_btn.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight");//.removeClass("ui-state-highlight")
                                    }
                                    //setColor_onTD(tmp_btn.parents("div.handsontable:first").parent());
                                }
                            } else {
                                var tmp_identity = null;
                                try { tmp_identity = "btn_" + column_setting.data; } catch (ignore) { }
                                var tmp_btn = $("<button class='multi_chk_all'></button>").prop("header_identity", tmp_identity).addClass(tmp_identity).button({
                                    icons: { primary: "ui-icon-blank" },
                                    label: "全選",
                                    text: false
                                }).on("mousedown", function (event) {
                                    event.preventDefault();
                                    event.stopPropagation();
                                }).on("click.multi_chk_all", function (event) {
                                    var need_select_all = !tmp_btn.hasClass("ui-state-highlight");
                                    /*if (need_select_all) {
                                        tmp_btn.button("option", { icons: { primary: 'ui-icon-check' } }).addClass("ui-state-highlight");//.addClass("ui-state-highlight")
                                    } else {
                                        tmp_btn.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight");//.removeClass("ui-state-highlight")
                                        //instance.deselectCell();
                                    }//20160204*/
                                    //var tmp_colHeader = tmp_btn.parents("span.colHeader:first");
                                    column_setting.header_selected = need_select_all;
                                    //setColor_onAllButton(need_select_all, tmp_btn);

                                    var tmp_arr = [];
                                    for (var tmp_r = 0; tmp_r < instance.countRows() ; tmp_r++) {
                                        tmp_arr.push([tmp_r, col, "" + need_select_all]);
                                    }
                                    instance.setDataAtCell(tmp_arr);//setDataAtCell cause afterGetColHeader event trigger (if afterGetColHeader trigger setDataAtCell cause infinite loop)

                                    //20160204 setColor_onAllCheckbox(need_select_all, tmp_btn, instance);

                                    //2020-03-08 volcan add, prevent autopostback
                                    return false;
                                });
                                if ($.isFunction(tmp_function)) {
                                    tmp_btn.on("click.click_header_function", tmp_function);
                                }
                                column_setting.header_button = tmp_btn;
                                if (tmp_show) {
                                    //tmp_colHeader.empty();
                                    is_htRight ? tmp_colHeader.append(tmp_btn) : tmp_colHeader.prepend(tmp_btn);//each afterGetColHeader cause tmp_colHeader reset to default html(選)
                                }
                            }
                        } else if (column_setting.data == "BC") {
                            var is_select_all = false;

                            var tmp_show = false;
                            var column_readOnly = false;
                            try { column_readOnly = column_setting.readOnly; } catch (ignore) { }
                            if (instance.getSettings().readOnly || column_readOnly) {
                                tmp_show = false;
                            } else {
                                for (var tmp_r = 0; tmp_r < instance.countRows() ; tmp_r++) {
                                    if (instance.getDataAtCell(tmp_r, col) != "R") {
                                        tmp_show = true;
                                        break;
                                    }
                                }
                            }
                            if (column_setting.header_button != null) {
                                var tmp_btn = column_setting.header_button;
                                is_select_all = !!column_setting.header_selected;
                                if (tmp_show) {
                                    tmp_colHeader.empty();
                                    tmp_colHeader.prepend(tmp_btn);
                                    if (is_select_all) {
                                        tmp_btn.button("option", { icons: { primary: 'ui-icon-check' } }).addClass("ui-state-highlight");//.addClass("ui-state-highlight")
                                    } else {
                                        tmp_btn.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight");//.removeClass("ui-state-highlight")
                                    }
                                    //setColor_onTD(tmp_btn.parents("div.handsontable:first").parent());//tag
                                }
                            } else {
                                var tmp_btn = $("<button class='btn_select_all'></button>").button({
                                    icons: { primary: "ui-icon-blank" },
                                    label: "全選",
                                    text: false
                                }).on("mousedown", function (event) {
                                    event.preventDefault();
                                    event.stopPropagation();
                                }).on("click.btn_select_all", function (event) {
                                    var need_select_all = !tmp_btn.hasClass("ui-state-highlight");
                                    /*if (need_select_all) {
                                        tmp_btn.button("option", { icons: { primary: 'ui-icon-check' } }).addClass("ui-state-highlight");//.addClass("ui-state-highlight")
                                    } else {
                                        tmp_btn.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight");//.removeClass("ui-state-highlight")
                                        //instance.deselectCell();
                                    }//20160204*/
                                    column_setting.header_selected = need_select_all;

                                    var tmp_arr = [];
                                    for (var tmp_r = 0; tmp_r < instance.countRows() ; tmp_r++) {
                                        if (instance.getDataAtCell(tmp_r, col) != "R") {
                                            tmp_arr.push([tmp_r, col, "" + need_select_all]);
                                        }
                                    }
                                    instance.setDataAtCell(tmp_arr);//setDataAtCell cause afterGetColHeader event trigger (if afterGetColHeader trigger setDataAtCell cause infinite loop)

                                    //20160204 setColor_onAllButton(need_select_all, tmp_btn, instance);
                                });
                                if ($.isFunction(tmp_function)) {
                                    tmp_btn.on("click.click_header_function", tmp_function);
                                }
                                column_setting.header_button = tmp_btn;
                                if (tmp_show) {
                                    tmp_colHeader.empty();
                                    tmp_colHeader.prepend(tmp_btn);//each afterGetColHeader cause tmp_colHeader reset to default html(選)
                                }
                            }
                        }
                    } else if (column_setting.data == "BA") {
                        //console.log("BA");
                        if (column_setting.header_button != null) {
                            var tmp_btn = column_setting.header_button;
                            tmp_colHeader.empty();
                            tmp_colHeader.prepend(tmp_btn);
                        } else {
                            $(elem).off("click", "button.btn_add");//prevent $(elem).on("click", "button.btn_add", function (event) { Ht.alter("insert_row", Ht.getSelected()[0]); });
                            var tmp_btn = $("<button class='btn_add_one'></button>").button({
                                icons: { primary: "ui-icon-plus" },
                                label: "增",
                                text: false
                            }).on("mousedown", function (event) {
                                event.preventDefault();
                                event.stopPropagation();
                            }).on("click.btn_add_one", function (event) {
                                instance.alter("insert_row", 0);
                            });
                            if ($.isFunction(tmp_function)) {
                                tmp_btn.on("click.click_header_function", tmp_function);
                            }
                            column_setting.header_button = tmp_btn;
                            tmp_colHeader.empty();
                            tmp_colHeader.prepend(tmp_btn);//each afterGetColHeader cause tmp_colHeader reset to default html(選)
                        }
                    }
                    /* else if (column_setting.type == "btn") {
                        if (tmp_colHeader.prop("header_tag") != null) {
                        } else {
                            //$(elem).off("click");//prevent $(elem).on("click", "button.btn_xxx", function (event) { });
                            tmp_colHeader.prop("header_tag", "header_tag");
                        }
                    }*/
                }
            },
        });
            /*afterRender: function (isForced) {
                var instance = this;
                //setColor_onTD
                var tmp_arr = $.extend([], instance.getDataAtProp("BC"), instance.getDataAtProp("BO"));
                if (tmp_arr.length > 0) {
                    $(instance.rootElement).find("tr:has(td)").each(function (idx,elem) {
                        var tmp_r = instance.getCoords($(elem).find("td:first").get(0)).row;
                        var checked = false;
                        if (!!tmp_arr[tmp_r]) {
                            checked = !!$.parseJSON(tmp_arr[tmp_r]);
                        }
                        if (checked) {
                            $(elem).find("td").addClass("ht_selected_row");
                        } else {
                            $(elem).find("td").removeClass("ht_selected_row");
                        }
                    });
                }
            },*/
            /*afterSelectionEnd: function (row, col, row2, col2) {
                //console.log("afterSelectionEnd");
                var instance = this;
                var container = instance.rootElement;
                if (row >= 0 && col >= 0 && row == row2 && col == col2 && instance.getSettings().data.length > 0) {
                    if ($(container).find("button:focus").length > 0) {
                        $(container).find("button:focus").trigger("blur");
                    }
                }
            },*/
    });
});
(function (Handsontable) {
    var ButtonRenderer = function (instance, TD, row, col, prop, value, cellProperties) {
        //var tmp_colHeader = $(instance.rootElement).find("TH span.colHeader:not(.cornerHeader):eq("+col+")");
        //instance.getSettings().columns[col].readOnly = true;//cell readOnly//In Renderer runtime, instance.getCoords(TD).row != row
        //console.log(instance.getSettings());
        var tmp_text = "", tmp_class = null, highlight_class = null, highlight_row_class = null, tmp_label = "", tmp_icon = "ui-icon-blank";
        try { tmp_label = "" + instance.getSettings().columns[col].header; } catch (ignore) { }
        switch (prop) {
            case "BA":
                tmp_text = "增";
                tmp_class = "btn_add";
                tmp_icon = "ui-icon-plus";
                break;

            case "BC":
                tmp_text = "選";
                tmp_class = "btn_chk";
                //tmp_icon = "ui-icon-blank";
                tmp_label = "選取";
                break;

            case "BD"://delete row
                tmp_text = "刪";
                tmp_class = "btn_del";
                tmp_icon = "ui-icon-closethick";
                tmp_label = "刪除";
                break;

            case "BK"://form submit
                tmp_text = "刪";
                tmp_class = "btn_del";
                tmp_icon = "ui-icon-closethick";
                tmp_label = "刪除";
                break;

            case "BM":
                tmp_text = "修";
                tmp_class = "btn_mod";
                tmp_icon = "ui-icon-wrench";
                tmp_label = "修改";
                break;

            case "BM2":
                tmp_text = "修";
                tmp_class = "btn_mod";
                tmp_icon = "ui-icon-gear";
                tmp_label = "修改";
                break;

            case "BO":
                tmp_text = "選";
                tmp_class = "btn_opt";
                //tmp_icon = "ui-icon-blank";
                tmp_label = "單選";
                break;

            case "BR":
                tmp_text = "上傳";
                tmp_class = "btn_upl";
                tmp_icon = "ui-icon-arrowthick-1-n";
                break;

            case "BU":
                tmp_text = "上傳";
                tmp_class = "btn_upl";
                tmp_icon = "ui-icon-arrowthick-1-n";
                break;

            case "BQ":
                tmp_text = "查";
                tmp_class = "btn_det";
                tmp_icon = "ui-icon-zoomin";
                tmp_label = "檢視";
                break;

            case "BQ1":
                tmp_text = "查";
                tmp_class = "btn_det1";
                tmp_icon = "ui-icon-zoomin";
                break;

            case "BQ2":
                tmp_text = "查";
                tmp_class = "btn_det2";
                tmp_icon = "ui-icon-zoomin";
                break;

            case "BQ3":
                tmp_text = "查";
                tmp_class = "btn_det3";
                tmp_icon = "ui-icon-zoomin";
                break;

            case "CP":
                tmp_text = "複";
                tmp_class = "btn_copy";
                tmp_icon = "ui-icon-copy";
                tmp_label = "複製";
                break;

            case "BP":
                tmp_text = "印";
                tmp_icon = "ui-icon-print";
                tmp_label = "列印";
                break;

            case "PC":
                tmp_text = "密碼";
                tmp_icon = "ui-icon-locked";
                tmp_label = "變更密碼";
                break;

            default:
                tmp_text = "按鈕";
        }
        //value=instance.getDataAtRowProp(row , prop)
        if (prop == "BD") { //volcan add
            highlight_class = "ui-state-error";
        }
        if(prop=="BO" || prop=="BC"){
            var checked = false;
            if (!!value) {
                checked = (value == "true");
                //try { checked = !!$.parseJSON(value); } catch (ignore) { }
            }
            if (checked) {
                tmp_icon = "ui-icon-check";
                highlight_class = "ui-state-highlight";
                highlight_row_class = "ht_checked_row";
            }
        }
        Handsontable.renderers.BaseRenderer.apply(this, arguments);
        var tmp_btn = $(TD).find("button.btn_" + prop);
        if (tmp_btn.length > 0) {
            if (prop == "BO" || prop == "BC") {
                if (!checked) {
                    tmp_btn.removeClass("ui-state-highlight").removeClass("ht_checked_row");
                }
                tmp_btn.button({
                    icons: { primary: tmp_icon },
                }).addClass(highlight_class).addClass(highlight_row_class);
            }
            return TD;
        }
        $(TD).empty().css("text-align", "left");
        if (instance.rootElement.btn_prepare == null) {
            instance.rootElement.btn_prepare = $('<button></button>');
        }
        var tmp_btn = instance.rootElement.btn_prepare.clone();
        tmp_btn.text(tmp_text).addClass(tmp_class).addClass("btn_" + prop).addClass(highlight_class).addClass(highlight_row_class);
        var column_readOnly = false;
        try { column_readOnly = instance.getSettings().columns[col].readOnly; } catch (ignore) { }
        if ((instance.getSettings().readOnly || column_readOnly || value == "R") && tmp_text != "查") {
            tmp_btn.attr("disabled", true);
            tmp_btn.addClass("ui-state-disabled");
        }
        tmp_btn.button({
            icons: { primary: tmp_icon },
            label: tmp_label,
            text: false
        /*}).on("focusin", function (event) {
            var tmp_arr = instance.getSelected();
            var tmp_r = instance.getCoords(TD).row;
            var tmp_c = instance.getCoords(TD).col;
            if (tmp_arr == null || tmp_arr[0] != tmp_r || tmp_arr[2] != tmp_r || tmp_arr[1] != tmp_c || tmp_arr[3] != tmp_c) {
                event.preventDefault();
                event.stopPropagation();
                instance.selectCell(tmp_r, tmp_c, tmp_r, tmp_c, true);
            }*/
        }).on("click.btn_cell_selected", function (event) {
            var tmp_arr = instance.getSelected();
            var tmp_r = instance.getCoords(TD).row;
            var tmp_c = instance.getCoords(TD).col;
            if (tmp_arr != null) {
                if (tmp_arr[0] != tmp_r || tmp_arr[2] != tmp_r || tmp_arr[1] != tmp_c || tmp_arr[3] != tmp_c) {
                    event.preventDefault();
                    event.stopPropagation();
                    tmp_btn.trigger("blur");
                    instance.selectCell(tmp_arr[0], tmp_arr[1], tmp_arr[0], tmp_arr[1], true);
                    return false;
                }
            }
        });
        switch (prop) {
            case "BA":
                tmp_btn.on("click.btn_add", function (event) {
                    var tmp_r = instance.getCoords(TD).row;
                    var tmp_c = instance.getCoords(TD).col;
                    instance.alter("insert_row", tmp_r + 1);
                    instance.selectCell(tmp_r, tmp_c, tmp_r, tmp_c, true);
                    $(window).trigger('resize');
                });
                break;
            case "BD":
                tmp_btn.on("click.btn_del", function (event) {
                    var tmp_r = instance.getCoords(TD).row;
                    instance.alter("remove_row", tmp_r);
                });
                break;
            case "BO":
            case "BC":
                tmp_btn.on("click.btn_renderer", function (event) {
                    var tmp_arr = instance.getSelected();
                    var tmp_r = instance.getCoords(TD).row;
                    var tmp_c = instance.getCoords(TD).col;
                    var is_selected = !!tmp_btn.hasClass("ui-state-highlight");
                    if (is_selected) {
                        instance.getSettings().columns[col].header_selected = !is_selected;
                    }
                    /*if (is_selected) {
                        tmp_btn.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight").removeClass("ht_checked_row");//.removeClass("ui-state-highlight")
                        tmp_btn.parents("div.handsontable:first").parent().find("button.btn_select_all").button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight").parents("span.colHeader").prop("select_all", !is_selected);
                    } else {
                        if (tmp_btn.hasClass("btn_opt")) {//單選需清空其他已選
                            tmp_btn.parents("div.handsontable:first").find("button.btn_opt").not(tmp_btn).button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight").removeClass("ht_checked_row");//.removeClass("ui-state-highlight")
                        }
                        tmp_btn.button("option", { icons: { primary: 'ui-icon-check' } }).addClass("ui-state-highlight").addClass("ht_checked_row");//.addClass("ui-state-highlight")
                    }//20160204*/
                    if (instance.getDataAtRowProp(row, prop) != "R") {
                        var tmp_array = [];
                        if (tmp_btn.hasClass("btn_opt") && !is_selected) {//單選需清空其他已選
                            for (var tmp_row = 0; tmp_row < instance.countRows() ; tmp_row++) {
                                if (tmp_row != tmp_r && instance.getDataAtRowProp(tmp_row, prop) == "true") {
                                    tmp_array.push([tmp_row, prop, "false"]);
                                    break;
                                }
                            }
                        }
                        tmp_array.push([tmp_r, prop, "" + !is_selected]);
                        instance.setDataAtRowProp(tmp_array);//setDataAtCell cause afterGetColHeader event trigger (if afterGetColHeader trigger setDataAtCell cause infinite loop)
                    }
                    if (tmp_arr == null || tmp_arr[0] != tmp_r || tmp_arr[2] != tmp_r || tmp_arr[1] != tmp_c || tmp_arr[3] != tmp_c) {
                        instance.selectCell(tmp_r, tmp_c, tmp_r, tmp_c, true);
                    }
                    //if (!instance.getSettings().columns[col].select_all) {
                    //    setColor_onTD(tmp_btn.parents("div.handsontable:first"));//tag
                    //}
                });
                break;
            default:
        }
        var tmp_function = null;
        try { tmp_function = instance.getSettings().columns[col].click_function; } catch (ignore) { }
        if ($.isFunction(tmp_function)) {
            tmp_btn.on("click.click_function_" + prop, tmp_function);
        }
        tmp_btn.appendTo(TD);
        return TD;
    };
    Handsontable.renderers.ButtonRenderer = ButtonRenderer;

    var ButtonEditor = Handsontable.editors.BaseEditor.prototype.extend();
    /*ButtonEditor.prototype.init = function () {
        Handsontable.editors.BaseEditor.prototype.init.apply(this, arguments);
        this.isCellEdited = false;
    };*/
    ButtonEditor.prototype.getValue = function () { };
    ButtonEditor.prototype.setValue = function (newValue) { };
    ButtonEditor.prototype.open = function () { };
    ButtonEditor.prototype.close = function () { };
    ButtonEditor.prototype.focus = function () {
        var TD = this.TD;
        if (!$(TD).find("button").is(":focus")) {
            $(TD).find("button").trigger("focus");
        }
    };
    ButtonEditor.prototype.beginEditing = function (initialValue, event) {
        this.focus();
        return;
    };
    ButtonEditor.prototype.finishEditing = function (revertToOriginal, ctrlDown, callback) {
        return;
    };
    Handsontable.editors.ButtonEditor = ButtonEditor;

    Handsontable.ButtonCell = {
        editor: Handsontable.editors.ButtonEditor,
        renderer: Handsontable.renderers.ButtonRenderer
    };
    $.extend(Handsontable.cellTypes, {
        btn: Handsontable.ButtonCell
    });
})(Handsontable);
(function (Handsontable) {
    var CheckboxRenderer = function (instance, TD, row, col, prop, value, cellProperties) {
        var tmp_text = "", tmp_class = null, highlight_class = null, tmp_label = "", tmp_icon = "ui-icon-blank";
        try { tmp_label = "" + instance.getSettings().columns[col].header; } catch (ignore) { }
        tmp_class = "btn_" + prop;
        tmp_text = tmp_label;
        var checked = false;
        if (!!value) {
            checked = (value == "true");
            //try { checked = !!$.parseJSON(value); } catch (ignore) { }
        }
        if (checked) {
            tmp_icon = "ui-icon-check";
            highlight_class = "ui-state-highlight";
        }
        Handsontable.renderers.BaseRenderer.apply(this, arguments);
        var tmp_btn = $(TD).find("button.btn_" + prop);
        if (tmp_btn.length > 0) {
            if (!checked) {
                tmp_btn.removeClass("ui-state-highlight");
            }
            tmp_btn.button({
                icons: { primary: tmp_icon },
            }).addClass(highlight_class);
            return TD;
        }
        $(TD).empty().css("text-align", "left");
        //instance.getSettings().columns[col].readOnly = true;//cell readOnly//In Renderer runtime, instance.getCoords(TD).row != row
        //console.log(instance.getSettings());

        if (instance.rootElement.btn_prepare == null) {
            instance.rootElement.btn_prepare = $('<button></button>');
        }
        tmp_btn = instance.rootElement.btn_prepare.clone();
        tmp_btn.text(tmp_text).addClass(highlight_class).addClass("multi_chk").addClass(tmp_class).prop("header_identity", tmp_class);
        var cell_readOnly = null;
        try { cell_readOnly = instance.getSettings().columns[col].vs_initialForm.D; } catch (ignore) { }
        var column_readOnly = false;
        try { column_readOnly = instance.getSettings().columns[col].readOnly; } catch (ignore) { }
        if ((instance.getSettings().readOnly || column_readOnly || cell_readOnly) && tmp_text != "查") {
            tmp_btn.attr("disabled", true);
            tmp_btn.addClass("ui-state-disabled");
        }
        tmp_btn.button({
            icons: { primary: tmp_icon },
            label: tmp_label,
            text: false
        /*}).on("focusin", function (event) {
            var tmp_arr = instance.getSelected();
            var tmp_r = instance.getCoords(TD).row;
            var tmp_c = instance.getCoords(TD).col;
            if (tmp_arr == null || tmp_arr[0] != tmp_r || tmp_arr[2] != tmp_r || tmp_arr[1] != tmp_c || tmp_arr[3] != tmp_c) {
                event.preventDefault();
                event.stopPropagation();
                instance.selectCell(tmp_r, tmp_c, tmp_r, tmp_c, true);
            }*/
        }).on("click.multi_chk", function (event) {
            var tmp_arr = instance.getSelected();
            var tmp_r = instance.getCoords(TD).row;
            var tmp_c = instance.getCoords(TD).col;
            if (tmp_arr != null) {
                if (tmp_arr[0] != tmp_r || tmp_arr[2] != tmp_r || tmp_arr[1] != tmp_c || tmp_arr[3] != tmp_c) {
                    event.preventDefault();
                    event.stopPropagation();
                    tmp_btn.trigger("blur");
                    instance.selectCell(tmp_arr[0], tmp_arr[1], tmp_arr[0], tmp_arr[1], true);
                    return false;
                }
            }
            var tmp_class = null;
            try { tmp_class = "."+tmp_btn.prop("header_identity"); } catch (ignore) { }
            //var tmp_editor = instance.getActiveEditor();
            var is_selected = !!tmp_btn.hasClass("ui-state-highlight");
            if (is_selected) {
                instance.getSettings().columns[col].header_selected = !is_selected;
            }
            /*if (is_selected) {
                tmp_btn.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight");
                tmp_btn.parents("div.handsontable:first").parent().find("button.multi_chk_all").filter(tmp_class).button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight").parents("span.colHeader").prop("select_all", !is_selected);
            } else {
                tmp_btn.button("option", { icons: { primary: 'ui-icon-check' } }).addClass("ui-state-highlight");
            }*/
            instance.setDataAtRowProp(tmp_r, prop, "" + !is_selected);
            if (tmp_arr == null || tmp_arr[0] != tmp_r || tmp_arr[2] != tmp_r || tmp_arr[1] != tmp_c || tmp_arr[3] != tmp_c) {
                instance.selectCell(tmp_r, tmp_c, tmp_r, tmp_c, true);
            }

            //2020-03-08 volcan add, prevent autopostback
            return false;
        });
        var tmp_function = null;
        try { tmp_function = instance.getSettings().columns[col].click_function; } catch (ignore) { }
        if ($.isFunction(tmp_function)) {
            tmp_btn.on("click.click_function_" + prop, tmp_function);
        }
        tmp_btn.appendTo(TD);
        return TD;
    };
    Handsontable.renderers.CheckboxRenderer = CheckboxRenderer;

    var CheckboxEditor = Handsontable.editors.BaseEditor.prototype.extend();
    /*CheckboxEditor.prototype.init = function () {
        Handsontable.editors.BaseEditor.prototype.init.apply(this, arguments);
        this.isCellEdited = false;
    };*/
    CheckboxEditor.prototype.getValue = function () { };
    CheckboxEditor.prototype.setValue = function (newValue) { };
    CheckboxEditor.prototype.open = function () { };
    CheckboxEditor.prototype.close = function () { };
    CheckboxEditor.prototype.focus = function () {
        var TD = this.TD;
        if (!$(TD).find("button").is(":focus")) {
            $(TD).find("button").trigger("focus");
        }
    };
    CheckboxEditor.prototype.beginEditing = function (initialValue, event) {
        this.focus();
        return;
    };
    CheckboxEditor.prototype.finishEditing = function (revertToOriginal, ctrlDown, callback) {
        return;
    };
    Handsontable.editors.CheckboxEditor = CheckboxEditor;

    Handsontable.CheckboxCell = {
        editor: Handsontable.editors.CheckboxEditor,
        renderer: Handsontable.renderers.CheckboxRenderer
    };
    $.extend(Handsontable.cellTypes, {
        multi_chk: Handsontable.CheckboxCell
    });
})(Handsontable);
(function (Handsontable) {
    //RocMonthRenderer start
    var RocMonthRenderer = function (instance, TD, row, col, prop, value, cellProperties) {
        //Handsontable.renderers.TextRenderer.apply(this, arguments);
        Handsontable.renderers.BaseRenderer.apply(this, arguments);
        //var tmp_td = $(TD).empty();
        var showValue = ("" + value).replace(/[^0-9]/g, "");
        showValue = showValue.substr(0,6);
        showValue = $.datepicker.ceDateToRoc(showValue);
        //$("<div></div>").appendTo(TD).text(showValue);
        Handsontable.Dom.fastInnerText(TD, showValue);
    };
    Handsontable.renderers.RocMonthRenderer = RocMonthRenderer;
    //RocMonthRenderer end

    //RocDateEditor start
    //https://github.com/handsontable/handsontable/wiki/Understanding-cell-editors
    //handsontable-0.12.5/src/editors/dateEditor.js
    var RocMonthEditor = Handsontable.editors.TextEditor.prototype.extend();
    RocMonthEditor.prototype.init = function () {
        if (typeof jQuery != 'undefined') {
            $ = jQuery;
        } else {
            throw new Error("You need to include jQuery to your project in order to use the jQuery UI Datepicker.");
        }

        if (!$.datepicker) {
            throw new Error("jQuery UI Datepicker dependency not found. Did you forget to include jquery-ui.custom.js or its substitute?");
        }

        Handsontable.editors.TextEditor.prototype.init.apply(this, arguments);

        this.isCellEdited = false;
        var that = this;

        this.instance.addHook('afterDestroy', function () {
            that.destroyElements();
        });

    };
    RocMonthEditor.prototype.createElements = function () {
        Handsontable.editors.TextEditor.prototype.createElements.apply(this, arguments);
        this.$datePicker = $("<div></div>").appendTo("body")
            .on("mousedown", function (event) { event.stopPropagation(); })
            .addClass("htDatepickerHolder");

        var that = this;
        var tmp_zIndex = 102;
        /*$(that.instance.rootElement).find("TH:visible:last").each(function (idx, elem) {
            console.log(idx + ":" + $(elem).zIndex());
            if ($(elem).zIndex() >= tmp_zIndex) {
                tmp_zIndex = $(elem).zIndex() + 1;
            }
            console.log(tmp_zIndex);
        });*/

        this.datePickerStyle = this.$datePicker.prop("style");
        this.datePickerStyle.position = 'absolute';
        this.datePickerStyle.top = 0;
        this.datePickerStyle.left = 0;
        this.datePickerStyle.zIndex = tmp_zIndex;
        this.datePickerStyle.display = 'none';//should not call this.hideDatepicker();

        //datepicker init
        this.$datePicker.datepicker($.extend({
            justYM:true,
            isRocYear:true,
            minDate: new Date(1813, 1 - 1, 1),
            maxDate: new Date(2910, 12 - 1, 31),
            buttonText:'民國年月選取',
            showAnim:''
        }, {
            dateFormat: 'yymm',
            /*beforeShow: function (input, inst) {//$(this)===inst.input
                var default_date=inst.input.datepicker("option","defaultDate");//datepicker defaultDate(Date or string or Number)
                var date=default_date;
                if(date instanceof String){
                    try{date=$.datepicker.parseDate("yymmdd",date);}catch(ignore){}//server side default
                }
                if(!(date instanceof Date) || isNaN(default_date)){
                    date=new Date();//client side default
                }
                if(!(default_date instanceof Date) || isNaN(default_date)){
                    default_date=date;
                }
				
                var yymm=inst.input.val();//form value(String)
                var default_yymm=""+inst.input.prop("defaultValue");//form default(String)
				
                date=null;
                try{date=$.datepicker.parseDate("yymmdd",default_yymm+"01");}catch(ignore){}
                if(date==null){//form default not valid
                    default_yymm=$.datepicker.formatDate("yymm",default_date);
                    inst.input.prop("defaultValue",default_yymm);//fix form default
                }
				
                date=null;
                try{date=$.datepicker.parseDate("yymmdd",yymm+"01");}catch(ignore){}
                if(date==null){//form value not valid
                    yymm=default_yymm;
                    try{date=$.datepicker.parseDate("yymmdd",yymm+"01");}catch(ignore){}
                }
				
                inst.input.datepicker("option","defaultDate",date);//overwrite defaultDate hack//cause onChangeMonthYear								
				
                var roc_yymm=$.datepicker.ceDateToRoc(yymm+"01").substr(0,5);
                inst.input.val(yymm).prop("rocMonthPickerView").val(roc_yymm);//fix form value
                
            },*/
            onChangeMonthYear: function(year, month, inst){
                var date = new Date(inst.selectedYear, inst.selectedMonth, 1);
                var yymm = $.datepicker.formatDate('yymm', date);
                var roc_yymm = $.datepicker.ceDateToRoc(yymm + "01").substr(0, 5);
                //$.datepicker._setYearMonth(inst);
                that.setValue(roc_yymm);
            },
            showOn: "focus",
            //onSelect: function (dateText, inst) {
                //that.setValue($.datepicker.ceDateToRoc(dateText));
                //that.setValue(dateText);
                //that.finishEditing(false);
            //},
            justYM:true,
            isRocYear:true
        }));
        /*
        this.$datePicker.datepicker($.extend({
            isRocYear: true,
            minDate: new Date(1813, 1 - 1, 1),
            maxDate: new Date(2910, 12 - 1, 31),
            buttonText: '民國日期選取',
            showAnim: ''
        }, {
            dateFormat: 'yymmdd',
            showOn: "focus",
            onSelect: function (dateText, inst) {
                //that.setValue($.datepicker.ceDateToRoc(dateText));
                that.setValue(dateText);
                that.finishEditing(false);
            },
            isRocYear: true
        }));*/
    };
    RocMonthEditor.prototype.prepare = function (row, col, prop, td, originalValue, cellProperties) {
        Handsontable.editors.TextEditor.prototype.prepare.apply(this, arguments);
        var formObj = this.TEXTAREA;
        $(formObj).vs_initialForm({ M: 5, N: 1 });
    }
    RocMonthEditor.prototype.destroyElements = function () {
        this.$datePicker.datepicker('destroy');
        this.$datePicker.remove();
    };
    RocMonthEditor.prototype.open = function () {
        Handsontable.editors.TextEditor.prototype.open.apply(this, arguments);
        this.showDatepicker();
    };
    RocMonthEditor.prototype.close = function () {
        this.hideDatepicker();
        Handsontable.editors.TextEditor.prototype.close.apply(this, arguments);
    };
    RocMonthEditor.prototype.beginEditing = function (initialValue, event) {
        var tmp_editor=this;
        initialValue = typeof initialValue == 'string' ? initialValue : this.originalValue;
        var ceDateString = ("" + initialValue).replace(/[^0-9]/g, "");
        if (tmp_editor.instance.getSettings().columns[tmp_editor.col].type == "rocym" && ceDateString == "") {
            ceDateString = $.datepicker.formatDate('yymm', (new Date()));
        }
        ceDateString = ceDateString.substr(0, 6);
        if ($.datepicker.checkCEDateValid(ceDateString,"yymm")) {
            initialValue = $.datepicker.ceDateToRoc(ceDateString);
        } else {
            initialValue = "";
        }
        Handsontable.editors.TextEditor.prototype.beginEditing.apply(this, arguments);
    };
    RocMonthEditor.prototype.finishEditing = function (revertToOriginal, ctrlDown, callback) {
        var showValue = this.getValue();
        var dateString = ("" + showValue).replace(/[^0-9]/g, "");
        dateString = dateString.substr(0, 6);
        if (dateString.length == 6 && $.datepicker.checkCEDateValid(dateString, "yymm")) {
            showValue = $.datepicker.ceDateToRoc(dateString);
        } else if ($.datepicker.checkRocDateValid(dateString, "yyymm")) {
            dateString = $.datepicker.rocDateToCE(dateString);
        } else {
            dateString = "";
            //showValue = "";
        }
        this.setValue(dateString);//prepare dateString to saveValue()
        Handsontable.editors.TextEditor.prototype.finishEditing.apply(this, arguments);//internally call saveValue()
        this.setValue(showValue);//holding showValue before re-render by RocMonthRenderer
    };
    RocMonthEditor.prototype.showDatepicker = function () {
        //console.log(tmp_editor.cellProperties);
        var tmp_editor = this;
        /*var offset = this.TD.getBoundingClientRect(),
          DatepickerSettings,
          datepickerSettings;


        this.datePickerStyle.top = (window.pageYOffset + offset.top + Handsontable.Dom.outerHeight(this.TD)) + 'px';
        this.datePickerStyle.left = (window.pageXOffset + offset.left) + 'px';

        DatepickerSettings = function () { };
        DatepickerSettings.prototype = this.cellProperties;
        datepickerSettings = new DatepickerSettings();
        */
        var ceDateString = ("" + tmp_editor.originalValue).replace(/[^0-9]/g, "");
        ceDateString = ceDateString.substr(0, 6);
        var tmp_date = new Date();
        if ($.datepicker.checkCEDateValid(ceDateString, "yymm")) {
            tmp_date = $.datepicker.parseDate("yymmdd", ceDateString + "01");
        }
        tmp_editor.$datePicker.datepicker('setDate', tmp_date);
        /*if ($.datepicker.checkCEDateValid(ceDateString)) {
            datepickerSettings.defaultDate = ceDateString;
        } else {
            datepickerSettings.defaultDate = $.datepicker.formatDate("yymmdd", new Date());
        }
        this.$datePicker.datepicker('setDate', datepickerSettings.defaultDate);
        this.$datePicker.datepicker('option', datepickerSettings);*/

        tmp_editor.datePickerStyle.display = '';
        tmp_editor.$datePicker.position({
            my: "left top",
            at: "right top",
            of: tmp_editor.TD,
            //collision :"flipfit",
        });
    };
    RocMonthEditor.prototype.hideDatepicker = function () {
        this.datePickerStyle.display = 'none';
    };
    Handsontable.editors.RocMonthEditor = RocMonthEditor;
    //RocMonthEditor end

    //RocMonthValidator start
    var RocMonthValidator = function (value, callback) {
        //this.allowInvalid = false;//hijack focus
        callback(value.length<=6 && $.datepicker.checkCEDateValid(value));
        //callback(true);//Valid
        //callback(false);//not Valid
    };
    Handsontable.RocMonthValidator = RocMonthValidator;
    //RocMonthValidator end

    Handsontable.RocMonthCell = {
        editor: Handsontable.editors.RocMonthEditor,
        renderer: Handsontable.renderers.RocMonthRenderer,
        validator: Handsontable.RocMonthValidator
    };
    Handsontable.RocMonthMaybeEmptyCell = {
        editor: Handsontable.editors.RocMonthEditor,
        renderer: Handsontable.renderers.RocMonthRenderer
    };
    $.extend(Handsontable.cellTypes, {
        rocym: Handsontable.RocMonthCell,
        rocym_or_empty: Handsontable.RocMonthMaybeEmptyCell
    });
})(Handsontable);
(function (Handsontable) {
    //RocDateRenderer start
    var RocDateRenderer = function (instance, TD, row, col, prop, value, cellProperties) {
        //Handsontable.renderers.TextRenderer.apply(this, arguments);
        Handsontable.renderers.BaseRenderer.apply(this, arguments);
        //var tmp_td = $(TD).empty();
        var showValue = ("" + value).replace(/[^0-9]/g, "");
        showValue = $.datepicker.ceDateToRoc(showValue);
        //$("<div></div>").appendTo(TD).text(showValue);
        Handsontable.Dom.fastInnerText(TD, showValue);
    };
    Handsontable.renderers.RocDateRenderer = RocDateRenderer;
    //RocDateRenderer end

    //RocDateEditor start
    //https://github.com/handsontable/handsontable/wiki/Understanding-cell-editors
    //handsontable-0.12.5/src/editors/dateEditor.js
    var RocDateEditor = Handsontable.editors.TextEditor.prototype.extend();
    RocDateEditor.prototype.init = function () {
        if (typeof jQuery != 'undefined') {
            $ = jQuery;
        } else {
            throw new Error("You need to include jQuery to your project in order to use the jQuery UI Datepicker.");
        }

        if (!$.datepicker) {
            throw new Error("jQuery UI Datepicker dependency not found. Did you forget to include jquery-ui.custom.js or its substitute?");
        }

        Handsontable.editors.TextEditor.prototype.init.apply(this, arguments);

        this.isCellEdited = false;
        var that = this;

        this.instance.addHook('afterDestroy', function () {
            that.destroyElements();
        });

    };
    RocDateEditor.prototype.createElements = function () {
        Handsontable.editors.TextEditor.prototype.createElements.apply(this, arguments);
        this.$datePicker = $("<div></div>").appendTo("body")
            .on("mousedown", function (event) { event.stopPropagation(); })
            .addClass("htDatepickerHolder");

        var that = this;
        var tmp_zIndex = 102;
        /*$(that.instance.rootElement).find("TH:visible:last").each(function (idx, elem) {
            console.log(idx + ":" + $(elem).zIndex());
            if ($(elem).zIndex() >= tmp_zIndex) {
                tmp_zIndex = $(elem).zIndex() + 1;
            }
            console.log(tmp_zIndex);
        });*/

        this.datePickerStyle = this.$datePicker.prop("style");
        this.datePickerStyle.position = 'absolute';
        this.datePickerStyle.top = 0;
        this.datePickerStyle.left = 0;
        this.datePickerStyle.zIndex = tmp_zIndex;
        this.datePickerStyle.display = 'none';//should not call this.hideDatepicker();
        

        //datepicker init
        this.$datePicker.datepicker($.extend({
            isRocYear: true,
            minDate: new Date(1813, 1 - 1, 1),
            maxDate: new Date(2910, 12 - 1, 31),
            buttonText: '民國日期選取',
            showAnim: ''
        }, {
            dateFormat: 'yymmdd',

            showOn: "focus",
            onSelect: function (dateText, inst) {
                //that.setValue($.datepicker.ceDateToRoc(dateText));
                that.setValue(dateText);
                that.finishEditing(false);
            },
            isRocYear: true
        }));
    };
    RocDateEditor.prototype.prepare = function (row, col, prop, td, originalValue, cellProperties) {
        Handsontable.editors.TextEditor.prototype.prepare.apply(this, arguments);
        var formObj = this.TEXTAREA;
        $(formObj).vs_initialForm({M:8,N:1});
    }
    RocDateEditor.prototype.destroyElements = function () {
        this.$datePicker.datepicker('destroy');
        this.$datePicker.remove();
    };
    RocDateEditor.prototype.open = function () {
        Handsontable.editors.TextEditor.prototype.open.apply(this, arguments);
        this.showDatepicker();
    };
    RocDateEditor.prototype.close = function () {
        this.hideDatepicker();
        Handsontable.editors.TextEditor.prototype.close.apply(this, arguments);
    };
    RocDateEditor.prototype.beginEditing = function (initialValue, event) {
        var tmp_editor = this;
        initialValue = typeof initialValue == 'string' ? initialValue : this.originalValue;
        var ceDateString = ("" + initialValue).replace(/[^0-9]/g, "");
        if (tmp_editor.instance.getSettings().columns[tmp_editor.col].type == "rocdate" && ceDateString == "") {
            ceDateString = $.datepicker.formatDate('yymmdd', (new Date()));
        }
        if ($.datepicker.checkCEDateValid(ceDateString)) {
            initialValue=$.datepicker.ceDateToRoc(ceDateString);
        } else {
            initialValue="";
        }
        Handsontable.editors.TextEditor.prototype.beginEditing.apply(this, arguments);
    };
    RocDateEditor.prototype.finishEditing = function (revertToOriginal, ctrlDown, callback) {
        var showValue = this.getValue();
        var dateString = ("" + showValue).replace(/[^0-9]/g, "");
        if (dateString.length == 8 && $.datepicker.checkCEDateValid(dateString,"yymmdd")) {
            showValue = $.datepicker.ceDateToRoc(dateString);
        } else if ($.datepicker.checkRocDateValid(dateString,"yyymmdd")) {
            dateString = $.datepicker.rocDateToCE(dateString);
        } else {
            dateString = "";
            //showValue = "";
        }
        this.setValue(dateString);//prepare dateString to saveValue()
        Handsontable.editors.TextEditor.prototype.finishEditing.apply(this, arguments);//internally call saveValue()
        this.setValue(showValue);//holding showValue before re-render by RocDateRenderer
    };
    RocDateEditor.prototype.showDatepicker = function () {
        //console.log(tmp_editor.cellProperties);
        var tmp_editor = this;
        /*var offset = this.TD.getBoundingClientRect(),
          DatepickerSettings,
          datepickerSettings;


        this.datePickerStyle.top = (window.pageYOffset + offset.top + Handsontable.Dom.outerHeight(this.TD)) + 'px';
        this.datePickerStyle.left = (window.pageXOffset + offset.left) + 'px';

        DatepickerSettings = function () { };
        DatepickerSettings.prototype = this.cellProperties;
        datepickerSettings = new DatepickerSettings();
        */
        var ceDateString = ("" + tmp_editor.originalValue).replace(/[^0-9]/g, "");
        var tmp_date = new Date();
        if ($.datepicker.checkCEDateValid(ceDateString)) {
            tmp_date = $.datepicker.parseDate("yymmdd", ceDateString);
        }
        tmp_editor.$datePicker.datepicker('setDate', tmp_date);
        /*if ($.datepicker.checkCEDateValid(ceDateString)) {
            datepickerSettings.defaultDate = ceDateString;
        } else {
            datepickerSettings.defaultDate = $.datepicker.formatDate("yymmdd", new Date());
        }
        this.$datePicker.datepicker('setDate', datepickerSettings.defaultDate);
        this.$datePicker.datepicker('option', datepickerSettings);*/

        tmp_editor.datePickerStyle.display = '';
        tmp_editor.$datePicker.position({
            my: "left top",
            at: "right top",
            of: tmp_editor.TD,
            //collision :"flipfit",
        });
    };
    RocDateEditor.prototype.hideDatepicker = function () {
        this.datePickerStyle.display = 'none';
    };
    Handsontable.editors.RocDateEditor = RocDateEditor;
    //RocDateEditor end

    //RocDateValidator start
    var RocDateValidator = function (value, callback) {
        //this.allowInvalid = false;//hijack focus
        callback($.datepicker.checkCEDateValid(value));
        //callback(true);//Valid
        //callback(false);//not Valid
    };
    Handsontable.RocDateValidator = RocDateValidator;
    //RocDateValidator end

    Handsontable.RocDateCell = {
        editor: Handsontable.editors.RocDateEditor,
        renderer: Handsontable.renderers.RocDateRenderer,
        validator:Handsontable.RocDateValidator
    };
    Handsontable.RocDateMaybeEmptyCell = {
        editor: Handsontable.editors.RocDateEditor,
        renderer: Handsontable.renderers.RocDateRenderer
    };
    $.extend(Handsontable.cellTypes, {
        rocdate: Handsontable.RocDateCell,
        rocdate_or_empty: Handsontable.RocDateMaybeEmptyCell
    });

})(Handsontable);
//auto resize handsontable column width

function handsontableResize(container, ColSet, resetVisibleColumns) {
    var instance = container.handsontable('getInstance');
    if (resetVisibleColumns == true) {
        container.removeProp("handsontable_visibleColSet");
    }
    var new_settings={ colWidths: void (0) };
    if (!container.prop("handsontable_visibleColSet")) {
        new_settings = $.extend(new_settings, {
            colHeaders: $.map(ColSet, function (elm, idx) { return (elm.header && !elm.hidden) ? elm.header : null; }),
            columns:$.grep(ColSet, function (elm, idx) { return (elm.header != null && !elm.hidden); }),
        });
    }
    instance.updateSettings(new_settings);//{ colWidths: void (0) }回預設值
    if (!container.is(":visible")) {
        return;
    }
    var visibleColSet = null;
    if (container.prop("handsontable_visibleColSet")) {
        visibleColSet = container.prop("handsontable_visibleColSet");
    }else{
        visibleColSet = ColSet;
        if (container.handsontable("countVisibleCols") != ColSet.length) {
            visibleColSet = $.grep(ColSet, function (elm, idx) { return (elm.header != null && !elm.hidden); });//filter
        }
        container.prop("handsontable_visibleColSet", visibleColSet);
    }
    var autoColumnSize_plugin = instance.getPlugin('autoColumnSize');
    autoColumnSize_plugin.recalculateAllColumnsWidth();//re-calculate width from data width
    var dynamicWidths = $.extend([], autoColumnSize_plugin.widths);
    if (dynamicWidths.length <= 0) {
        if (container.prop("handsontable_dynamicWidths")) {
            dynamicWidths = container.prop("handsontable_dynamicWidths");
        } else {
            dynamicWidths = [];
        }
    }
    //console.log("dynamicWidths");
    //console.log(dynamicWidths);
    var fixedWidths = null;
    var fixedWidths_sum = null;
    if (container.prop("handsontable_fixedWidths")) {
        fixedWidths = container.prop("handsontable_fixedWidths");
        fixedWidths_sum = container.prop("handsontable_fixedWidths_sum");
    } else {
        fixedWidths = [];
        fixedWidths_sum = 0;
        $.each(visibleColSet, function (idx, elem) {
            if (!isNaN(elem.width)) {
                fixedWidths[idx] = elem.width;
            } else if (elem.type == "select") {
                var tmp_select_max_width = instance.getColWidth(idx)-0;
                var tmp_prop = instance.colToProp(idx);
                if (typeof tmp_prop == "string") {
                    $(instance.rootElement).find("select.sel_" + tmp_prop).each(function () {
                        if (this.length == 1 && this.value == "") {
                            return;
                        } else if (this.length <= 0) {
                            return;
                        }
                        var tmp_select_width = $(this).width() || 0;
                        tmp_select_max_width = Math.max(tmp_select_max_width, tmp_select_width);
                    });
                }
                fixedWidths[idx] = tmp_select_max_width + 15;
            }
            if (fixedWidths[idx] != null) {
                fixedWidths_sum += (fixedWidths[idx] - 0);
            }
        });
        container.prop("handsontable_fixedWidths", fixedWidths);
        container.prop("handsontable_fixedWidths_sum", fixedWidths_sum);
    }
    $.each(fixedWidths, function (idx, val) {
        if (!isNaN(val) && dynamicWidths.length - 1 >= idx) {
            delete dynamicWidths[idx];
        }
    });
    //console.log(dynamicWidths);
    if (dynamicWidths.length > 0) {
        container.prop("handsontable_dynamicWidths", dynamicWidths);
    }
    var dynamicWidths_sum = 0;
    $.each(dynamicWidths, function (idx, val) {
        if (!isNaN(val)) {
            dynamicWidths_sum += (val - 0);
        }
    });
    /*console.log(fixedWidths);
    console.log(dynamicWidths);
    console.log(fixedWidths_sum);
    console.log(dynamicWidths_sum);*/

    var totalWidths = $.extend([], fixedWidths, dynamicWidths);
    var totalWidths_sum = fixedWidths_sum + dynamicWidths_sum;
    /*console.log(totalWidths);
    console.log(totalWidths_sum);*/

    var rowHeader_width = 0;
    if (container.find("col.rowHeader:first").length > 0) {
        rowHeader_width = container.find("col.rowHeader:first").width();
    }
    var window_width_without_scrollbar = $(window).innerWidth() || $(window).width();
    //instance.updateSettings({ colWidths: void (0) });//回預設值
    var doc_width = $(document).width() + $(document).scrollLeft();
    doc_width = Math.max(window_width_without_scrollbar, doc_width);
    var expand_width = doc_width - rowHeader_width;
    if (container.width() > rowHeader_width && (container.width() - rowHeader_width) < expand_width) {
        expand_width = container.width() - rowHeader_width;
    }

    var expand_factor = 1;
    if (expand_width > totalWidths_sum) {
        var share_width = expand_width - fixedWidths_sum;
        if (share_width > dynamicWidths_sum && dynamicWidths_sum > 0) {
            expand_factor = share_width / dynamicWidths_sum;
        }
    }
    //console.log(expand_factor);
    //if (expand_factor > 1) {
        var dynamicWidths_extend = [];
        $.each(dynamicWidths, function (idx, val) {
            if (!isNaN(val)) {
                dynamicWidths_extend[idx] = Math.floor(val * expand_factor);
            }
        });
        finalWidths = $.extend([], fixedWidths, dynamicWidths_extend);
        instance.updateSettings({ colWidths: finalWidths });//調整
    //} else {
    //    if (instance.getSettings().colWidths != null) {//已放大
    //        instance.updateSettings({ colWidths: void (0) });//回預設值
    //    }
    //}

};
/*function handsontableResize_init(container, ColSet) {
    if (!container.prop("handsontable_resize_init_width_array")) {
        var visibleColSet = ColSet;
        if (container.handsontable("countVisibleCols") != ColSet.length) {
            visibleColSet = $.grep(ColSet, function (elm, idx) { return (elm.header != null); });//filter
        }
        var init_ignore_array = $.map(visibleColSet, function (elm, idx) { return isNaN(elm.width) ? null : elm.data; });//init_ignore_array:["BQ", "BM", "BC", "BD"]
        var init_total_width = 0, init_ignore_width = 0, init_width_array = [];
        $.each(visibleColSet, function (idx, elm) {
            elemWidth = container.handsontable("getColWidth", idx);
            init_total_width += elemWidth;
            if ($.inArray(("" + elm.data), init_ignore_array) != -1) {
                init_ignore_width += elemWidth;
            }
            init_width_array[""+elm.data] = elemWidth;
        });//container.handsontable('getInstance').getPlugin('autoColumnSize').widths <=> init_width_array,autoColumnSize live until colWidths setup
        container.prop("handsontable_resize_ignore_column_data", init_ignore_array);//init_ignore_array:["BQ", "BM", "BC", "BD"]
        container.prop("handsontable_resize_column_width_sum", init_total_width);
        container.prop("handsontable_resize_ignore_width_sum", init_ignore_width);
        container.prop("handsontable_resize_init_width_array", init_width_array);
        container.prop("handsontable_visibleColSet", visibleColSet);
    }
};
function handsontableResize(container, ColSet) {
    if (!container.prop("handsontable_resize_init_width_array")) {
        handsontableResize_init(container, ColSet);
    }
    var visibleColSet=container.prop("handsontable_visibleColSet");
    var rowHeader_width = 0;
    if (container.find("col.rowHeader:first").length>0) {
        rowHeader_width = container.find("col.rowHeader:first").width();
    }
    var init_instance = container.handsontable("getInstance");
    var init_width_array = container.prop("handsontable_resize_init_width_array");
    var expand_width_array = [];
    var window_width_without_scrollbar = window.innerWidth || $(window).width();
    var expand_width = window_width_without_scrollbar - rowHeader_width;
    if (container.width() > rowHeader_width && (container.width() - rowHeader_width) < expand_width) {
        expand_width = container.width() - rowHeader_width;
    }
    var expand_factor = 1;
    if (expand_width > container.prop("handsontable_resize_column_width_sum")) {
        var share_width = expand_width - container.prop("handsontable_resize_ignore_width_sum")-3;
        var base_width = container.prop("handsontable_resize_column_width_sum") - container.prop("handsontable_resize_ignore_width_sum");
        if (share_width <= base_width) { share_width = base_width; }
        expand_factor = share_width / base_width;
    }
    $.each(visibleColSet, function (idx, elm) {
        var col_expand_factor = 1;
        if ($.inArray(("" + elm.data), container.prop("handsontable_resize_ignore_column_data")) == -1) {
            col_expand_factor = expand_factor;
        }
        expand_width_array[idx] = (init_width_array["" + elm.data] * col_expand_factor).toFixed(0) - 0;
    });
    init_instance.updateSettings({ colWidths: expand_width_array });
    //init_instance.render();//updateSettings() cause render()
    //setColor_onTD(container);
};*/

//按鈕選取變色
function setColor_onButton(event) {//將取消
};
/*function setColor_onSingleButton(event) {
    var tmp_btn = $(this);
    var is_selected = !!tmp_btn.hasClass("ui-state-highlight");
    if (is_selected) {
        tmp_btn.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight").removeClass("ht_checked_row");//.removeClass("ui-state-highlight")
        tmp_btn.parents("div.handsontable:first").parent().find("button.btn_select_all").button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight").parents("span.colHeader").prop("select_all", !is_selected);
    } else {
        if (tmp_btn.hasClass("btn_opt")) {//單選需清空其他已選
            tmp_btn.parents("div.handsontable:first").find("button.btn_opt").not(tmp_btn).button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight").removeClass("ht_checked_row");//.removeClass("ui-state-highlight")
        }
        tmp_btn.button("option", { icons: { primary: 'ui-icon-check' } }).addClass("ui-state-highlight").addClass("ht_checked_row");//.addClass("ui-state-highlight")
    }
    setColor_onTD(tmp_btn.parents("div.handsontable:first"));
};*/
/*function setColor_onTD(tmp_container) {
    var tmp_checked_btn = tmp_container.find("button.ui-state-highlight");
    tmp_container.find("button").not(tmp_checked_btn).parent("td").siblings("td").addBack().removeClass("ht_selected_row");
    tmp_checked_btn.parent("td").siblings("td").addBack().addClass("ht_selected_row");
    var tmp_arr = [];
    if (tmp_checked_btn.length == 0) {
        var instance=tmp_container.handsontable('getInstance');
        tmp_arr = $.extend([], instance.getDataAtProp("BC"));
        console.log(tmp_arr);
    }
};*/
/*function setColor_onAllButton(is_select_all, tmp_btn, instance) {
    if (typeof (is_select_all) == "boolean" && !!tmp_btn && ("" + tmp_btn.prop("tagName")).toLowerCase() == "button") {
        var tmp_container = tmp_btn.parents("div.handsontable:first").parent();
        var tmp_all_btn = tmp_container.find("button.btn_chk:not(:disabled)");
        if (is_select_all) {
            tmp_all_btn.button("option", { icons: { primary: 'ui-icon-check' } }).addClass("ui-state-highlight").addClass("ht_checked_row");
        }else{
            tmp_all_btn.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight").removeClass("ht_checked_row");
        }
    }
};//20160204*/
/*function setColor_onAllCheckbox(is_select_all, tmp_btn, instance) {
    if (typeof (is_select_all) == "boolean" && !!tmp_btn && ("" + tmp_btn.prop("tagName")).toLowerCase() == "button") {
        var tmp_container = tmp_btn.parents("div.handsontable:first").parent();
        var identity_class = null;
        try { identity_class = "." + tmp_btn.prop("header_identity"); } catch (ignore) { }
        var tmp_all_btn = tmp_container.find("button.multi_chk:not(:disabled)").filter(identity_class);
        if (is_select_all) {
            tmp_all_btn.button("option", { icons: { primary: 'ui-icon-check' } }).addClass("ui-state-highlight");
        } else {
            tmp_all_btn.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight");
        }
    }
};//20160204*/
function setColor_onSelect_BUTTON_DEL(event) {
    var tmp_btn = $(this);
    if (tmp_btn.hasClass("ui-state-highlight")) {
        tmp_btn.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight");//.removeClass("ui-state-highlight")
    } else {
        tmp_btn.button("option", { icons: { primary: 'ui-icon-closethick' } }).addClass("ui-state-highlight");//.addClass("ui-state-highlight")
    }
    setColor_onTD(tmp_btn.parents("div.handsontable:first"));
};

//回傳存檔 submit json string
function get_ht_submit_string(Type, HT, ColSet, InitData, posIdxInit) {
    var rtn = [];
    var SubmitCol = [];
    var KeyCol = [];
    var FunCol = [];

    $.each(ColSet, function (i, t) { if (t.submit == 1) { SubmitCol.push(t.data); } });
    $.each(ColSet, function (i, t) { if (t.submit == 1) { KeyCol.push(t.data); } });// && t.hidden
    $.each(ColSet, function (i, t) { if (!t.submit) { FunCol.push(t.data); } });// && !t.hidden
    
    Type = Type.toLowerCase();
    if (Type == "single") {
        var obj = {};
        $.each(SubmitCol, function (i, t) {
            obj[SubmitCol[i]] = $.trim(HT.getDataAtRowProp(HT.getSelected()[0], SubmitCol[i]));
        });
        rtn.push(obj);
    } else if (Type == "multi") {
        $(".btn_chk,.btn_opt").each(function (idx) {
            if ($(this).hasClass("ui-state-highlight")) {
                var obj = {};
                $.each(SubmitCol, function (i, t) {
                    obj[SubmitCol[i]] = $.trim(HT.getDataAtRowProp(idx, SubmitCol[i]));
                });
                rtn.push(obj);
            }
        });
    } else if (Type == "save") {
        for (var idx = 0; idx < HT.countRows() ; idx++) {
            if (!HT.isEmptyRow(idx)) {
                var obj = {};
                $.each(SubmitCol, function (i, t) {
                    obj[SubmitCol[i]] = $.trim(HT.getDataAtRowProp(idx, SubmitCol[i]));
                });
                rtn.push(obj);
            }
        }
    } else if (Type == "save2") {
        for (var idx = 0; idx < HT.countRows() ; idx++) {
            if (!HT.isEmptyRow(idx)) {

                var obj = {};
                //var Act = HT.getDataAtRowProp(idx, "IsMode").toString().trim();
                var modIdx = 0;
                var InitRow;
                var oldColV;
                var newColV;
                var RptCnt = 0;
                var Act;

                modIdx = HT.getDataAtRowProp(idx, "posIdx");
                InitRow = $.grep(InitData, function (i) { return i.posIdx == modIdx });
                Act = InitRow[0]["IsMode"].toString().trim();
                //I
                if (Act != "") {
                    if (modIdx > posIdxInit ) {
                        $.each(SubmitCol, function (i, t) {
                            newColV = HT.getDataAtRowProp(idx, SubmitCol[i]).toString().trim();
                            if (SubmitCol[i] != KeyCol[i]) {
                                if (newColV != null && newColV != "") {
                                    obj[SubmitCol[i]] = HT.getDataAtRowProp(idx, SubmitCol[i]);
                                } else {
                                    RptCnt++;
                                }
                            }
                            else {
                                obj[SubmitCol[i]] = HT.getDataAtRowProp(idx, KeyCol[i]);
                            }
                        });
                        if (RptCnt < (HT.countCols() - FunCol.length)) {
                            if (Act != "I") { Act = "I" };
                            obj["IsMode"] = Act;
                        } else {
                            Act = "";
                            InitData[idx]["IsMode"] = "";
                        }
                    }
                    //U
                    else if (modIdx >= 0 && modIdx <= posIdxInit) {
                        $.each(SubmitCol, function (i, t) {
                            oldColV = InitRow[0][SubmitCol[i]].toString().trim();
                            newColV = HT.getDataAtRowProp(idx, SubmitCol[i]).toString().trim();
                            if (SubmitCol[i] != KeyCol[i]) {
                                if (newColV != oldColV) {
                                    obj[SubmitCol[i]] = HT.getDataAtRowProp(idx, SubmitCol[i]);
                                } else {
                                    RptCnt++;
                                }
                            }
                            else {
                                obj[SubmitCol[i]] = HT.getDataAtRowProp(idx, KeyCol[i]);
                            }
                        });
                        if (RptCnt <= (HT.countCols() - FunCol.length)) {
                            //Act = "U";
                            if (Act != "U") { Act = "U" };
                            obj["IsMode"] = Act;
                        } else {
                            Act = "";
                            InitData[idx]["IsMode"] = Act;
                        }
                    }
                //I:insert, U:update
                if (("IU").indexOf(Act) > -1 && obj != null) { rtn.push(obj); }
                }
            }
        }
        //D
        var delData = $.grep(InitData, function (i) { return (i.IsMode == "D" && i.posIdx >= 0 && i.posIdx <= posIdxInit) });
        
        if (delData.length > 0) {
            for (var idx = 0; idx < delData.length ; idx++) {
                var obj = {};
                $.each(KeyCol, function (i, t) {
                    obj[KeyCol[i]] = delData[idx][KeyCol[i]];
                });
                rtn.push(obj);
            }
        }
    }
    return JSON.stringify(rtn);
}

//ht vs_formValidate
function ht_formValidate(Ht, HtColSet) {
    var formValidate = $.grep(HtColSet, function (t, i) { return t.vs_formValidate != null; })
    var massage = "";
    for (var i = 0; i < Ht.countRows() ; i++) {
        if (!Ht.isEmptyRow(i)) {
            $.each(formValidate, function (j, t) {
                var formObjValue = $.trim(Ht.getDataAtRowProp(i, formValidate[j]["data"]));
                var Validate = formValidate[j]["vs_formValidate"];
                $.each(Validate, function (name, value) {
                    switch (name) {
                        case "EI":
                            break;
                        case "NE":
                            if (!isNaN(value) && value - 0 > 0 && formObjValue == "") {
                                massage += "不可為空<br>";
                            }
                            break;
                        case "NL":
                            if (!isNaN(value) && value - 0 > 0 && formObjValue.length < value) {
                                massage += "長度需要至少" + value + "位<br>";
                            }
                            break;
                        case "ML":
                            if (!isNaN(value) && value - 0 > 0 && formObjValue.length > value) {
                                massage += "不可超過" + value + "位長度<br>";
                            }
                            break;
                        case "N":
                            if (!isNaN(value) && value - 0 > 0 && (formObjValue.match(/[^0-9\.]/) != null || isNaN(formObjValue - 0))) {
                                massage += "必須為0-9數字<br>";
                            }
                            break;
                        case "NQ":
                            if (!isNaN(value) && value - 0 > 0 && (formObjValue.match(/[^0-9-]/) != null || isNaN(formObjValue - 0))) {
                                massage += "必須為整數<br>";
                            }
                            break;
                        case "DP":
                            if (!isNaN(value) && value - 0 > 0 && (formObjValue.match(/[^0-9\.]/) != null || isNaN(formObjValue - 0))) {
                                massage += "必須為正數<br>";
                            }
                            break;
                        case "DC":
                            if (!isNaN(value) && value - 0 > 0 && (formObjValue.match(/[^0-9\.-]/) != null || isNaN(formObjValue - 0))) {
                                massage += "必須為數字<br>";
                            }
                            break;
                        case "EN":
                            if (value.match(/^(U)$/i) != null && formObjValue.match(/[^0-9A-Z]/) != null) {//EN-U
                                massage += "必須為大寫英數字<br>";
                            } else if (value.match(/^(L)$/i) != null && formObjValue.match(/[^0-9a-z]/) != null) {//EN-L
                                massage += "必須為小寫英數字<br>";
                            } else if (!isNaN(value) && value - 0 > 0 && formObjValue.match(/[^0-9A-Za-z]/) != null) {//EN
                                massage += "必須為英數字<br>";
                            }
                            break;
                        case "DT":
                            if (!isNaN(value) && value - 0 > 0) {
                                if (formObjValue.match(/^[0-9]{8}$/) == null) {
                                    massage += "必須為正確日期字串<br>";
                                } else {
                                    var tmpstrArray = formObjValue.match(/^([0-9]{4})([0-9]{2})([0-9]{2})$/);
                                    var tmpYYYY = tmpstrArray[1];
                                    var tmpMM = tmpstrArray[2];
                                    var tmpDD = tmpstrArray[3];
                                    var tmpDate = new Date("" + tmpMM + "/" + tmpDD + "/" + tmpYYYY);
                                    if (tmpYYYY - 0 < 1850 || tmpYYYY - 0 != tmpDate.getFullYear() || tmpMM - 1 != tmpDate.getMonth() || tmpDD - 0 != tmpDate.getDate()) {
                                        massage += "必須為正確日期字串<br>";
                                    }
                                }
                            }
                            break;
                        case "DT1":
                            if (!isNaN(value) && formObjValue.length > 0) {
                                if (formObjValue.match(/^[0-9]{4}[/][0-9]{2}[/][0-9]{2}$/) == null) {
                                    massage += "必須為正確日期字串<br>";
                                } else {
                                    var tmpstrArray = formObjValue.split("/");
                                    var tmpYYYY = tmpstrArray[0];
                                    var tmpMM = tmpstrArray[1];
                                    var tmpDD = tmpstrArray[2];
                                    var tmpDate = new Date("" + tmpMM + "/" + tmpDD + "/" + tmpYYYY);
                                    if (tmpYYYY - 0 < 1850 || tmpYYYY - 0 != tmpDate.getFullYear() || tmpMM - 1 != tmpDate.getMonth() || tmpDD - 0 != tmpDate.getDate()) {
                                        massage += "必須為正確日期字串<br>";
                                    }
                                }
                            }
                            break;
                        case "YM":
                            if (!isNaN(value) && value - 0 > 0) {
                                if (formObjValue.match(/^[0-9]{6}$/) == null) {
                                    massage += "必須為正確年月字串<br>";
                                } else {
                                    var tmpstrArray = formObjValue.match(/^([0-9]{4})([0-9]{2})$/);
                                    var tmpYYYY = tmpstrArray[1];
                                    var tmpMM = tmpstrArray[2];
                                    var tmpDate = new Date("" + tmpMM + "/01/" + tmpYYYY);
                                    if (tmpYYYY - 0 < 1850 || tmpYYYY - 0 != tmpDate.getFullYear() || tmpMM - 1 != tmpDate.getMonth()) {
                                        massage += "必須為正確年月字串<br>";
                                    }
                                }
                            }
                            break;
                        case "TP":
                            if (!isNaN(value) && value - 0 > 0) {
                                if (formObjValue.match(/^[0-2][0-9][:][0-5][0-9][:][0-5][0-9]$/) == null) {
                                    massage += "必須為正確時分秒字串<br>";
                                } else {
                                    var tmpstrArray = formObjValue.match(/^([0-2][0-9])[:]([0-5][0-9])[:]([0-5][0-9])$/);
                                    var tmpHOUR = tmpstrArray[1];
                                    var tmpMINUTE = tmpstrArray[2];
                                    var tmpSECOND = tmpstrArray[3];
                                    if (tmpHOUR - 0 > 23 || tmpMINUTE - 0 > 59 || tmpSECOND - 0 > 59) {
                                        massage += "必須為正確時分秒字串<br>";
                                    }
                                }
                            }
                            break;
                        case "PW":
                            if (!isNaN(value) && value - 0 > 0 && formObjValue.length > 0 && (formObjValue.length < 8 || !(formObjValue.match(/[a-zA-Z]/) != null && formObjValue.match(/[0-9]/) != null))) {
                                massage += "密碼需為英文、數字混合8位數以上<br>";
                            }
                            break;
                    }
                });

                if (massage != "") {
                    Ht.selectCellByProp(i, formValidate[j]["data"]);
                    massage = '第' + (i + 1) + '筆 ' + formValidate[j]["header"] + '<br>' + massage;
                    alert(massage.replace(/<br>/gi, '\n\n'));
                    return false;
                }
            });
            if (massage != "") {
                break;
            }
        }
    }
    return (massage == "" ? true : false);
}