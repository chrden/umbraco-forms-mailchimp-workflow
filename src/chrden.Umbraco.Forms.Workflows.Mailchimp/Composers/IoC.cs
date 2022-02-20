using chrden.Umbraco.Forms.Workflows.Mailchimp.Services;
using chrden.Umbraco.Forms.Workflows.Mailchimp.Workflow;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Forms.Core.Providers;

namespace chrden.Umbraco.Forms.Workflows.Mailchimp.Composers
{
    public class IoC : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<IMailchimpService, MailchimpService>();

            builder.WithCollectionBuilder<WorkflowCollectionBuilder>().Add<MailchimpWorkflow>();
        }
    }
}