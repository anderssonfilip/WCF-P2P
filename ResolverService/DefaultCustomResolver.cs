using System;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;

namespace ResolverService
{
    class ResolverService
    {
        public static void Main()
        {
            var crs = new CustomPeerResolverService {ControlShape = false};

            var customResolver = new ServiceHost(crs);

            crs.Open();
            customResolver.Open();

            Console.WriteLine("Custom resolver service is started");
            Console.WriteLine("Press <ENTER> to terminate service");
            Console.ReadLine();

            crs.Close();
            customResolver.Close();
        }
    }
}
