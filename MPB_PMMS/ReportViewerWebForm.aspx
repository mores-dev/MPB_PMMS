<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ReportViewerWebForm.aspx.cs" Inherits="ReportViewerForMvc.ReportViewerWebForm" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" /> 
    <title></title>
    <%--<link href="Content/print.min.css" rel="stylesheet" />--%>
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <%--<script src="Scripts/print.min.js"></script>--%>
    <%--<script src="/ReportViewer/telerikReportViewer-13.2.19.918.js"></script>--%>
    <style type="text/css">
        .msrs-printdialog-main {
            top:34px !important;
            left:100px !important
        }
    </style>
</head>
<body style="margin: 0px; padding: 0px">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="ReportViewerForMvc" Name="ReportViewerForMvc.Scripts.PostMessage.js" />
                </Scripts>
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
<script>
    jQuery.waitFor = function (check, timeout, checkInterval) {
        var dfd = jQuery.Deferred();
        var checkHandle = setInterval(function () {
            if (check()) {
                clearInterval(checkHandle);
                dfd.resolve();
            }
        }, checkInterval || 50);
        var timeoutHandle = setTimeout(function () {
            if (dfd.state() == "pending") {
                clearInterval(checkHandle);
                clearTimeout(timeoutHandle);
                dfd.reject();
            }
        }, timeout || 5000);
        return dfd.promise();
    };

    $(document).ready(function () {
        var state = false;
        function init() {
            state = false;
            console.log("Start");
        }
        function trigger() {
            state = true;
            console.log("Trigger");
        }

        $(".glyphui.glyphui-print").on("click", function (event) {
            init();
            $.waitFor(function () { return state; }, 300)
                .done(function () { console.log("done."); }).fail(function () {
                    $("p.msrs-printdialog-caption").html("Print");
                    $("p.msrs-printdialog-settingtext").html("We'll create a printer-friendly PDF version of your report.");
                    $("#msrs-printdialog-label-pagesize").html("Page size:");
                    $("#msrs-printdialog-label-pageorientation").html("Page orientation:");
                    
                    $(".msrs-printdialog-divbuttons.msrs-printdialog-divprintbutton.msrs-printdialog-divhighlightbutton").click();
                });
            
            if ($(".msrs-printdialog-main").css("display") == "none") {
                $("p.msrs-printdialog-settingtext").html("Test");
            }
        });

    });
</script>