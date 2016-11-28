using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {

            string eventHubName = "eventhub1";
            string eventHubConnectionString = "Endpoint=sb://jz-eventhub-1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=gWm6v5Ujou06FFfSa7B1Zw/hrtgEkGFcvWjfDCFk0vM=";

            string storageAccountName = "jziotstorage1";
            string storageAccountKey = "/Oo9KCoyXNZDBe5WB3Rn4fXA0IeWXUMFk7+7j2Io33OIVMBT6F+XeWVEhXHZFFY3OJMP+fJL8RzMD2ucZ1fnRQ==";
            string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", storageAccountName, storageAccountKey);

            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, eventHubName, EventHubConsumerGroup.DefaultGroupName, eventHubConnectionString, storageConnectionString);
            Console.WriteLine("Registering EventProcessor...");
            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>(options).Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();

        }
    }
}
