#pragma checksum "C:\Users\robin_naidoo\Desktop\WarehouseReservationSystem-master\WarehouseReservationSystem-master\WarehouseReservationSystem\Areas\GCCustomer\Views\Vessel\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "662a28733ae12494f0bce612b7ce8b6db67a6580"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_GCCustomer_Views_Vessel_Index), @"mvc.1.0.view", @"/Areas/GCCustomer/Views/Vessel/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\robin_naidoo\Desktop\WarehouseReservationSystem-master\WarehouseReservationSystem-master\WarehouseReservationSystem\Areas\GCCustomer\Views\_ViewImports.cshtml"
using WarehouseReservationSystem;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\robin_naidoo\Desktop\WarehouseReservationSystem-master\WarehouseReservationSystem-master\WarehouseReservationSystem\Areas\GCCustomer\Views\_ViewImports.cshtml"
using WarehouseReservationSystem.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"662a28733ae12494f0bce612b7ce8b6db67a6580", @"/Areas/GCCustomer/Views/Vessel/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"682f2fdd9b2db387c2c98b02c4e958a398ab6c70", @"/Areas/GCCustomer/Views/_ViewImports.cshtml")]
    public class Areas_GCCustomer_Views_Vessel_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<WarehouseReservationSystem.Models.GC.Vessel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
#nullable restore
#line 3 "C:\Users\robin_naidoo\Desktop\WarehouseReservationSystem-master\WarehouseReservationSystem-master\WarehouseReservationSystem\Areas\GCCustomer\Views\Vessel\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/NewLayout/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<style>
    .switch {
        position: relative;
        display: inline-block;
        width: 60px;
        height: 34px;
    }

        .switch input {
            opacity: 0;
            width: 0;
            height: 0;
        }

    .slider {
        position: absolute;
        cursor: pointer;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: #ccc;
        -webkit-transition: .4s;
        transition: .4s;
    }

        .slider:before {
            position: absolute;
            content: """";
            height: 26px;
            width: 26px;
            left: 4px;
            bottom: 4px;
            background-color: white;
            -webkit-transition: .4s;
            transition: .4s;
        }

    input:checked + .slider {
        background-color: #2196F3;
    }

    input:focus + .slider {
        box-shadow: 0 0 1px #2196F3;
    }

    input:checked + .slider:before {
        -webkit-transform: translateX(26px);
        -ms-transform: translateX(");
            WriteLiteral(@"26px);
        transform: translateX(26px);
    }

    /* Rounded sliders */
    .slider.round {
        border-radius: 34px;
    }

        .slider.round:before {
            border-radius: 50%;
        }
</style>

<link rel=""stylesheet"" href=""https://cdn.datatables.net/buttons/1.7.1/css/buttons.dataTables.min.css"" />
<link rel=""stylesheet"" href=""https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css"" />
<link rel=""stylesheet"" href=""https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.css"" />
<link rel=""stylesheet"" href=""https://cdnjs.cloudflare.com/ajax/libs/icheck-bootstrap/3.0.1/icheck-bootstrap.min.css"" integrity=""sha512-8vq2g5nHE062j3xor4XxPeZiPjmRDh6wlufQlfC6pdQ/9urJkU07NM0tEREeymP++NczacJ/Q59ul+/K2eYvcg=="" crossorigin=""anonymous"" referrerpolicy=""no-referrer"" />




<br />
<div class=""p-4 border rounded"">
    <table id=""Table01"" class=""table table-striped table-bordered"" style=""width:100%; table-layout:fixed"">
        <thead class=""thead-dark"">
            <tr class=""table-inf");
            WriteLiteral(@"o"">
                <th>Vessel No.</th>
                <th>Voyage No.</th>
                <th>Name</th>
                <th>Is Active</th>
                <th></th>
            </tr>
        </thead>
    </table>
</div>

<script src=""https://cdn.datatables.net/buttons/1.7.1/js/dataTables.buttons.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js""></script>
<script src=""https://cdn.datatables.net/buttons/1.7.1/js/buttons.html5.min.js""></script>
<script>

    Table01 = $(""#Table01"").DataTable({
        ""ajax"": {
            ""url"": ""/GCCustomer/Vessel/GetAll""
        },
        ""columns"": [
            { ""data"": ""vesselNo"", ""width"": ""100%"" },
            { ""data"": ""voyageNo"", ""width"": ""100%"" },
            { ""data"": ""name"", ""width"": ""100%"" },
            {
                ""data"": {
                    isActive: ""isActive""
                },

                ""render"": function (data) {

                    if (data.isActive == true) {
                        return `
");
            WriteLiteral(@"                            <div class=""text-center"">
                                <a onclick=IsActive(""/GCCustomer/Vessel/Deactivate/${data.id}"") class=""btn btn-success btn-lg btn-block text-white"" style=""cursor:pointer"" data-toggle=""tooltip"" data-placement=""top"" title=""Click to Turn Off"">
                                  <i class=""fas fa-power-off""></i>
                                </a>
                            </div>
                           `;
                    }

                    else {
                        return `
                            <div class=""text-center"">
                                <a onclick=IsActive(""/GCCustomer/Vessel/Deactivate/${data.id}"") class=""btn btn-danger btn-lg btn-block text-white"" style=""cursor:pointer"" data-toggle=""tooltip"" data-placement=""top"" title=""Click to Turn On"">
                                 <i class=""fas fa-power-off""></i>
                                </a>
                            </div>
                           `;
                ");
            WriteLiteral(@"    }


                }, ""width"": ""10%""

            },
            {
                ""data"": {
                    id: ""id""
                },
                ""render"": function (data) {

                    return `
                                                    <div class=""text-center"">
                                                        <a href=""/FruitCustomer/Booking/Edit/${data.id}"" class=""btn btn-success text-white"" style=""cursor:pointer"" data-toggle=""tooltip"" data-placement=""top"" title=""Edit Booking"">
                                                            <i class=""fas fa-edit""></i> 
                                                        </a>

                                                        <a href=""/FruitCustomer/Booking/Details/${data.id}"" class=""btn btn-primary text-white"" style=""cursor:pointer"" data-toggle=""tooltip"" data-placement=""top"" title=""View Details"">
                                                            <i class=""fas fa-info""></i> 
                           ");
            WriteLiteral(@"                             </a>

                                                        <a onclick=Delete(""/FruitCustomer/Booking/Delete/${data.id}"") class=""btn btn-danger text-white"" style=""cursor:pointer"" data-toggle=""tooltip"" data-placement=""top"" title=""Delete Booking"">
                                                            <i class=""fas fa-trash-alt""></i> 
                                                        </a>

                                                    </div>
                                                   `;

                }, ""width"": ""30%""
            }
        ]
    });




    function IsActive(url) {
        swal({
            title: ""Are you sure?"",
            icon: ""warning"",
            buttons: true,
            dangerMode: true
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: url,
                    success: function (data) {
                        if (data.success) {
                            toas");
            WriteLiteral(@"tr.success(data.message);
                            Table01.ajax.reload();
                        }
                        else {
                            toastr.error(data.message);
                        }
                    }
                });
            }
        });
    }

    

</script>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<WarehouseReservationSystem.Models.GC.Vessel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
