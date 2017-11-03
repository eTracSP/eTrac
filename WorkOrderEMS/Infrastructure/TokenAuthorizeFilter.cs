using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WorkOrderEMS.BusinessLogic.BusinessHelpers;

namespace WorkOrderEMS.Infrastructure
{
    public class TokenAuthorizeFilter : AuthorizeAttribute // AuthorizationFilterAttribute
    {

        //readonly string _connString = ConfigurationManager.AppSettings["SQLConnection"].ToString();
        //private workorderEMSEntities db = new workorderEMSEntities();
        public override void OnAuthorization(HttpActionContext filterContext)
        {

            if (AuthorizationChecking.Authorize(filterContext))
            {
                return;
            }
            else
            {

                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                                          new { Error = true, Message = "Token is invalid" });
                //filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                //filterContext.Response.Content = new StringContent("The token has been expired. Please login again.");
                return;
            }



        }



        //public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{
        //    if (actionContext.Request.Headers.Contains("ServiceAuthKey") && actionContext.Request.Headers.GetValues("ServiceAuthKey") != null)
        //    {
        //        // get value from header
        //        string authorizationHeader = Convert.ToString(actionContext.Request.Headers.GetValues("ServiceAuthKey").FirstOrDefault());

        //        //We have header. Now we need to format it and check in the database if it is found.
        //        string attemptedAccessToken = ExtractAccessToken(authorizationHeader);


        //        bool authorizationApproved = false;

        //        bool tokenExpired = false;

        //        //using (LuckyMoney.DAL.LuckyMoneyIdentityContext identityContext = new LuckyMoney.DAL.LuckyMoneyIdentityContext())
        //        //{
        //        //    //var matchedResult = identityContext.AccessTokens.Where(at => at.AccessToken == attemptedAccessToken).FirstOrDefault();

        //        //    var matchedResult = AccountHelper.MatchAccessTokens(attemptedAccessToken);
        //        //    if (matchedResult != null)
        //        //    {
        //        //        authorizationApproved = true;

        //        //        // userID = matchedResult.UserID;
        //        //        SenderId = matchedResult.SenderId;

        //        //        tokenExpired = matchedResult.ExpiresOn <= DateTime.Now;
        //        //    }
        //        //}
        //        if (authorizationApproved)
        //        {
        //            if (!tokenExpired)
        //            {
        //                //SetCurrentUserSender(SenderId);
        //                return;
        //            }
        //            else
        //            {
        //                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        //                actionContext.Response.Content = new StringContent("The token has been expired. Please login again.");
        //                return;
        //            }
        //        }
        //    }

        //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
        //    actionContext.Response.Content = new StringContent("The requested resource could not be accessed.");
        //    return;
        //}

        //private string ExtractAccessToken(string authorizationHeader)
        //{
        //    string accessToken = string.Empty;

        //    if (authorizationHeader.ToLower().Contains("bearer"))
        //    {
        //        string[] splitResult = authorizationHeader.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        //        if (splitResult.Count() == 2)
        //        {
        //            accessToken = splitResult[1].Trim();
        //        }
        //    }

        //    return accessToken;
        //}
    }
}