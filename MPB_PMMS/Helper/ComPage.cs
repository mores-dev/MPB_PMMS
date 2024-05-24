using MPB_Entities.COMMON;
//using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MPB_PMMS.Helper
{
    /// <summary>
    /// 通用頁面共用程式
    /// </summary>
    public class ComPage
    {
        /// <summary>
        /// 產生下拉選單option html
        /// </summary>
        /// <param name="optionData">下拉選單Option的Text與Value</param>
        /// <param name="defaultSelectValue">預選值</param>
        /// <param name="optionSplit">如果optionSplit為空白時只顯示Name,否則顯示Code+optionSplit+Name</param>
        /// <returns></returns>
        public static string GetDropdownList(List<CodeName> optionData, string defaultSelectValue, string optionSplit)
        {
            StringBuilder renderHtmlTag = new StringBuilder();
            TagBuilder optionTag = new TagBuilder("option");
            renderHtmlTag.AppendLine(optionTag.ToString(TagRenderMode.Normal));

            foreach (var item in optionData)
            {
                optionTag = new TagBuilder("option");
                optionTag.Attributes.Add("value", item.Code);
                if (!string.IsNullOrEmpty(defaultSelectValue) && defaultSelectValue.Equals(item.Code))
                {
                    optionTag.Attributes.Add("selected", "selected");
                }
                if (string.IsNullOrEmpty(optionSplit))
                {
                    optionTag.SetInnerText(item.Name);
                }
                else
                {
                    optionTag.SetInnerText(item.Code + optionSplit + item.Name);
                }
                renderHtmlTag.AppendLine(optionTag.ToString(TagRenderMode.Normal));
            }
            return renderHtmlTag.ToString();
        }

        /// <summary>
        /// 產生勾選選單html(以List<CodeName>傳入勾選選單的值).
        /// </summary>
        /// <param name="name">勾選選單的name</param>
        /// <param name="optionData">勾選選單Option的Text與Value.</param>
        /// <param name="defaultSelectValue">預選值.</param>
        /// <param name="optionSplit">如果optionSplit為空白時只顯示Name,否則顯示Code+optionSplit+Name</param>
        /// <param name="IsSingle">是否單選</param>
        /// <returns></returns>
        static string GetCheckboxList(string name, List<CodeName> optionData, string[] defaultSelectValue, string optionSplit, bool IsSingle)//)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "產生CheckboxList時 tag Name 不得為空");
            }
            StringBuilder renderHtmlTag = new StringBuilder();
            foreach (var item in optionData)
            {
                TagBuilder input = new TagBuilder("input");
                input.Attributes.Add("name", name);
                input.Attributes.Add("type", "checkbox");
                input.Attributes.Add("value", item.Code);
                input.Attributes.Add("class", IsSingle ? "single_check" : "multiple_check");
                foreach (string s in defaultSelectValue ?? new string[0])
                {
                    if (s.Equals(item.Code))
                    {
                        input.Attributes.Add("checked", "checked");
                        break;
                    }
                }
                TagBuilder span = new TagBuilder("span");
                if (string.IsNullOrEmpty(optionSplit))
                {
                    span.InnerHtml = item.Name;
                }
                else
                {
                    span.InnerHtml = item.Code + optionSplit + item.Name;
                }


                renderHtmlTag.AppendLine(input.ToString(TagRenderMode.Normal) + span.ToString(TagRenderMode.Normal));
            }
            return renderHtmlTag.ToString();
        }

        /// <summary>
        /// 產生複選選單html(以List<CodeName>傳入勾選選單的值).
        /// </summary>
        /// <param name="name">勾選選單的name</param>
        /// <param name="optionData">勾選選單Option的Text與Value.</param>
        /// <param name="defaultSelectValue">預選值.</param>
        /// <param name="optionSplit">如果optionSplit為空白時只顯示Name,否則顯示Code+optionSplit+Name</param>
        /// <returns></returns>
        public static string GetCheckboxList(string name, List<CodeName> optionData, string[] defaultSelectValue, string optionSplit)//)
        {
            return GetCheckboxList(name, optionData, defaultSelectValue, optionSplit, false);
        }

        /// <summary>
        /// 產生單選選單html(以List<CodeName>傳入勾選選單的值).
        /// </summary>
        /// <param name="name">勾選選單的name</param>
        /// <param name="optionData">勾選選單Option的Text與Value.</param>
        /// <param name="defaultSelectValue">預選值.</param>
        /// <param name="optionSplit">如果optionSplit為空白時只顯示Name,否則顯示Code+optionSplit+Name</param>
        /// <returns></returns>
        public static string GetRadioboxList(string name, List<CodeName> optionData, string defaultSelectValue, string optionSplit)//)
        {
            string[] defValue;
            if (string.IsNullOrEmpty(defaultSelectValue))
            {
                defValue = new string[0];
            }
            else
            {
                defValue = new string[1];
                defValue[0] = defaultSelectValue;
            }
            return GetCheckboxList(name, optionData, defValue, optionSplit, true);
        }

        /// <summary>
        /// 民國日期格式化
        /// </summary>
        /// <param name="sourceDate">民國日期</param>
        /// <param name="type">格式化類別</param>
        /// 1:104年01月01日
        /// <returns></returns>
        public static string DspROCDate(string sourceDate, string type) {
            if (string.IsNullOrEmpty(sourceDate)) {
                sourceDate = "";
            }
            if (string.IsNullOrEmpty(type))
            {
                type = "1";
            }
            string sY = "";
            string sM = "";
            string sD = "";
            string newDate = sourceDate;
            if (sourceDate.Length == 7)
            {
                sY = sourceDate.Substring(0, 3);
                sM = sourceDate.Substring(3, 2);
                sD = sourceDate.Substring(5, 2);
            }
            else if (sourceDate.Length == 8)
            {
                sY = "" + (int.Parse(sourceDate.Substring(0, 4)) - 1911);
                sM = sourceDate.Substring(4, 2);
                sD = sourceDate.Substring(6, 2);
            }
            else
            {
                string[] tmpAry = sourceDate.Split('/');
                if (tmpAry.Length == 3) {
                    sY = tmpAry[0];
                    sM = tmpAry[1];
                    sD = tmpAry[2];
                    if (sY.Length == 4) {
                        sY = "" + (int.Parse(sourceDate.Substring(0, 4)) - 1911);
                    }
                }
            }
            if (!sY.Equals("")) {
                if (type.Equals("1")) {
                    newDate = sY + "年" + sM + "月" + sD + "日";
                }
            }

            return newDate;
        }
    }
}