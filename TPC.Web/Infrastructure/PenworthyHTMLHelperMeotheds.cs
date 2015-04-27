using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Common.Enumerations;

namespace TPC.Web.Infrastructure
{
    public static class PenworthyHTMLHelperMeotheds
    {
        public static string ConvertEnumToBindType(this HtmlHelper helper, string type)
        {
            string bindingTypeText = string.Empty;
            type = type != null ? type.Trim() : type;
            bindingTypeText = type == "CAT" ? Resources.TCPResources.categoryText :
                type == "PRO" ? Resources.TCPResources.PromotionalText :
                type == "PPB" ? Resources.TCPResources.PenworthyPreBoundBookText :
                type == "PUP" ? Resources.TCPResources.PuppetsText :
                type == "BB" ? Resources.TCPResources.BoardBookText :
                type == "LBY" ? Resources.TCPResources.LibraryBoundBookText : "";
            return bindingTypeText;
        }
        public static string ConvertEnumToProductLine(this HtmlHelper helper, string type)
        {
            string bindingTypeText = string.Empty;
            type = type != null ? type.Trim(): type;
            bindingTypeText = type == "CAT" ? Resources.TCPResources.categoryText :
                type == "PRO" ? Resources.TCPResources.PromotionalText :
                type == "PPB" ? "Prebound" :
                type == "PUP" ? Resources.TCPResources.PuppetsText :
                type == "BB" ? Resources.TCPResources.BoardBookText :
                type == "LBY" ? Resources.TCPResources.LibraryBoundBookText : "";
            return bindingTypeText;
        }
        public static string ConvertEnumToProductLineBinding(this HtmlHelper helper, string type)
        {
            string bindingTypeText = string.Empty;
            type = type != null ? type.Trim() : type;
            bindingTypeText = type == "CATT" || type == "CATB" || type == null ? "Cataloging" :
                type == "PPB" ? "Prebound" :
                type == "LBY" ? "Library" :
                type == "PRO" ? Resources.TCPResources.PromotionalText :                
                type == "PUP" ? Resources.TCPResources.PuppetsText :
                type == "BB" ? Resources.TCPResources.BoardBookText : "";
            return bindingTypeText;
        }
        public static string ConvertEnumToDWStatus(this HtmlHelper helper, string dwID)
        {
            string bindingTypeText = string.Empty;
            switch (dwID)
            {
                case "1":
                    bindingTypeText = DecisionWhizardStatusEnum.Yes.ToString();
                    break;
                case "2":
                    bindingTypeText = DecisionWhizardStatusEnum.No.ToString();
                    break;
                case "3":
                    bindingTypeText = DecisionWhizardStatusEnum.MayBe.ToString();
                    break;
                case "5":
                    bindingTypeText = DecisionWhizardStatusEnum.New.ToString();
                    break;
                default:
                    break;
            }

            //  c.DWSelectionID.ToString(),// == 1 ? DecisionWhizardStatusEnum.Yes.ToString(): c.DWSelectionID == 2 ? DecisionWhizardStatusEnum.No.ToString(): c.DWSelectionID == 3 ? DecisionWhizardStatusEnum.MayBe.ToString(): c.DWSelectionID == 5 ? DecisionWhizardStatusEnum.New.ToString():"",
            return bindingTypeText;
        }
        public static string ConvertUniversalDateTimeToLocalTime(this HtmlHelper helper, DateTime penworthyDateTime, DateTime customerDateTime)
        {
            //TimeSpan timeSpan = customerDateTime - customerDateTime.ToUniversalTime();
            //TimeZoneInfo custTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("");

            //Added to resolve error:"The conversion could not be completed because the supplied DateTime did not have the Kind property set correctly"
            DateTime dtpenworthyDateTime = new DateTime(penworthyDateTime.Ticks, DateTimeKind.Unspecified);
            //End Add

            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(dtpenworthyDateTime, TimeZoneInfo.Local);
            return String.Format("{0:g}", cstTime);
        }
    }
}