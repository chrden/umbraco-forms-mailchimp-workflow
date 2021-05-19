using CD.UmbracoFormsMailchimpWorkflow.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace CD.UmbracoFormsMailchimpWorkflow.Composers
{
    public class IoC : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<IMailchimpService, MailchimpService>();
        }
    }
}