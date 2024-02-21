using ImplicitlyMap.Example.Dtos;
using ImplicitlyMap.Example.Entity;
using ImplicitlyMap.Factories;
using ImplicitlyMap.Utilities;

namespace ImplicitlyMap
{
    internal class Program
    {      
        private static void Main(string[] args)
        {
            try
            {

                //For Example

                //var company = new Company()
                //{
                //  Name = "New Company",
                //  Address = "Şişli",
                //  Fax = "3432432"
                //};
                
                //var companyDto = new CompanyDto()
                //{
                //    Name = "New Company",
                //    Address = "Şişli",
                //    Fax = "3432432"
                //};

                //companyDto = company;


                var generator = AutoImplicitOperatorGeneratorFactory.Create()
                    .Configure(builder =>
                    {
                        builder.WithModelDirectory(ProjectDirectoryHelper.FindDirectoryByName(nameof(Example.Dtos)))
                            .WithUsingNamespace("ImplicitlyMap.Example.Entity")
                        .AddTargetTypeSuffix("Dto")
                        .AddTargetTypeSuffix("Model")
                        .AddTargetTypeSuffix("Request")
                        .AddTargetTypeSuffix("Response");
                    })
                    .GenerateImplicitOperators();


                Console.WriteLine("Implicit operators have been created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}