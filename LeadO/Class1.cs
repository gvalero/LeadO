using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
namespace LeadO
{
    public class pluginclass : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the execution context from the service provider.
            IPluginExecutionContext context =
                (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Get a reference to the Organization service.
            IOrganizationServiceFactory factory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            if (context.InputParameters != null)
            {
                //entity = (Entity)context.InputParameters["Target"];
                //Instead of getting entity from Target, we use the Image
                Entity entity = context.PostEntityImages["PostImage"];

                Money rate = (Money)entity.Attributes["po_rate"];
                int units = (int)entity.Attributes["po_units"];
                EntityReference parent = (EntityReference)entity.Attributes["po_parentid"];

                //Multiply
                Money total = new Money(rate.Value * units);

                //Set the update entity
                Entity parententity = new Entity("po_parententity");
                parententity.Id = parent.Id;
                parententity.Attributes["po_total"] = total;

                //Update
                service.Update(parententity);
            }
        }

    }
}
