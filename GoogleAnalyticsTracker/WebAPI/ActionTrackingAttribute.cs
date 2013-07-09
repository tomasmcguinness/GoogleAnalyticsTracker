using GoogleAnalyticsTracker.Web;
using System;
using System.Web.Http.Filters;

namespace GoogleAnalyticsTracker.WebAPI
{
    public class ActionTrackingAttribute
        : ActionFilterAttribute
    {
        public Tracker Tracker { get; set; }

        public string ActionDescription { get; set; }
        public string ActionUrl { get; set; }

        public ActionTrackingAttribute()
            : this(null, null, null, null)
        {
        }

        public ActionTrackingAttribute(string trackingAccount, string trackingDomain)
            : this(trackingAccount, trackingDomain, null, null)
        {
        }

        public ActionTrackingAttribute(string trackingAccount, string trackingDomain, string actionDescription, string actionUrl)
        {
            Tracker = new Tracker(trackingAccount, trackingDomain, new CookieBasedAnalyticsSession());
            ActionDescription = actionDescription;
            ActionUrl = actionUrl;
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            OnActionExecuted(actionContext);
        }

        public virtual string BuildCurrentActionName(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            return ActionDescription ??
                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + " - " +
                   filterContext.ActionDescriptor.ActionName;
        }

        public virtual string BuildCurrentActionUrl(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            var request = filterContext.Request;

            return ActionUrl ??
                   (request.RequestUri != null ? request.RequestUri.ToString() : "");
        }

        public virtual void OnActionExecuted(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            Tracker.TrackPageView(BuildCurrentActionName(filterContext), BuildCurrentActionUrl(filterContext));
        }
    }
}