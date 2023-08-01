using CustomInputControl;
using Microsoft.AspNetCore.Components.Forms;
using Northwind.Interface.Server.Adaptors;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Popups;
using System.Reflection;
using Action = Syncfusion.Blazor.Grids.Action;


namespace Northwind.Interface.Server.Pages
{
    public class SfOrdersModel<TLocal> : BaseComponentModel<TLocal>
    {
        protected CustomGridAddEditDel<OrderReturnView> orderGrid;
        protected CustomGridAddEditDel<OrderDetailReturnView> orderDeteilGrid;
        protected CustomDDList<int?, ProductReturnView, ProductsAdaprtor> ddlProducts { get; set; }
        protected OrderDetailReturnView orderDetail { get; set; }
        protected SfDialog sfDialog { get; set; }
        protected ProductsControl products { get; set; }

        protected OrderDetailReturnView SelectOrderDetail { get; set; } = new();
        protected OrderReturnView SelectOrder { get; set; }


        protected OrderDetailReturnView SelectedOrderDetail { get; set; }

        protected string filterCompanyName { get; set; }
        protected string filterContactName { get; set; }
        protected string filterEmplFullName { get; set; }
        protected double? filterFreight { get; set; }
        protected string filterShipName { get; set; }
        protected string filterShipAddress { get; set; }
        protected string filterShipCountry { get; set; }
        protected string filterShipCity { get; set; }
        protected string filterPostalCode { get; set; }
        protected double? filterTotalSumm { get; set; }
        protected DateTime? filterOrderDate { get; set; }
        protected DateTime? filterRequiredDate { get; set; }
        protected DateTime? filterShippedDate { get; set; }

        protected DialogSettings dialogSettings = new() { MinHeight = "550", Height = "550" };
        protected DialogSettings dialogOrderDetailSettings = new() { MinHeight = "350", Height = "350" };
        protected Query QueryOrderDetail { get; set; } = new() { Params = new Dictionary<string, object>() };
        protected Query DdlQuery { get; set; } = new() { Params = new Dictionary<string, object>() };
        protected string ExcludeProductIdsStr { get; set; }
        protected DataManager DmOrderDetail { get; set; }
        protected Dictionary<string, string> header { get; set; } = new();
        protected override void OnInitialized()
        {
            component.HeaderMessage = localResource["Title"];

        }

        protected async Task OnActionBegin(ActionEventArgs<OrderReturnView> ord)
        {
            try
            {

                if (ord.RequestType == Action.Filtering && ord.CurrentFilteringColumn != null)
                {
                    if (orderGrid.PageSettings.CurrentPage > 1)
                        await orderGrid.CustomGoToPage(1);
                }
                if (ord.RequestType == Action.Filtering && ord.CurrentFilteringColumn == null)
                {
                    if (ord.CurrentFilterObject != null)
                    {
                        typeof(SfOrders).GetProperty($"filter{ord.CurrentFilterObject.Field}", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, null);
                        await orderGrid.CustomFilterByColumnAsync(ord, null, ord.CurrentFilterObject.Field);
                    }
                }
                if (ord.RequestType == Action.Filtering && orderGrid != null)
                {
                    ord.Cancel = true;

                    switch (ord.CurrentFilteringColumn)
                    {
                        case nameof(OrderReturnView.CompanyName):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterCompanyName);
                            break;
                        case nameof(OrderReturnView.ContactName):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterContactName);
                            break;
                        case nameof(OrderReturnView.EmplFullName):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterEmplFullName);
                            break;
                        case nameof(OrderReturnView.Freight):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterFreight);
                            break;
                        case nameof(OrderReturnView.ShipName):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterShipName);
                            break;
                        case nameof(OrderReturnView.ShipAddress):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterShipAddress);
                            break;
                        case nameof(OrderReturnView.Country):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterShipCountry);
                            break;
                        case nameof(OrderReturnView.City):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterShipCity);
                            break;
                        case nameof(OrderReturnView.PostalCode):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterPostalCode);
                            break;
                        case nameof(OrderReturnView.TotalSumm):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterTotalSumm);
                            break;
                        case nameof(OrderReturnView.OrderDate):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterOrderDate);
                            break;
                        case nameof(OrderReturnView.ShippedDate):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterShippedDate);
                            break;
                        case nameof(OrderReturnView.RequiredDate):
                            await orderGrid.CustomFilterByColumnAsync(ord, filterRequiredDate);
                            break;
                    }
                }
                if (orderGrid != null && orderGrid.removeDialog != null)
                    if (ord.RequestType == Syncfusion.Blazor.Grids.Action.Delete && orderGrid.removeDialog.Flag)
                    {
                        ord.Cancel = true;  //Cancel default delete action.
                        await orderGrid.removeDialog.ShowRemoveMessage($" {ord.Data.CompanyName}");
                        orderGrid.removeDialog.Flag = false;
                    }
            }
            catch (Exception ex)
            {
                await component.ShowErrorMessage(ex.Message);
            }
        }

        protected async Task OnRecordClick(RowSelectingEventArgs<OrderReturnView> order)
        {
            try
            {
                SelectOrder = order.Data;
                QueryOrderDetail = new Query().AddParams(Constans.OrderId, order.Data.Id);
                //await orderDeteilGrid.RefreshCustom(new Query().AddParams("Id", order.Data.Id));
                if (DmOrderDetail != null)
                    ExcludeProductIdsStr = await ExcluseProductIds();
            }
            catch (Exception ex) { await component.ShowErrorMessage(ex.Message); }
            // orderDeteilGrid.Query = new Query().AddParams("Id", order.Data.Id);
            // await orderDeteilGrid.Refresh();

            //await orderGrid.ExpandCollapseDetailRowAsync(order.Data);
        }
        protected void OrderExpanded(DetailsExpandedEventArgs<OrderReturnView> order)
        {
            SelectOrder = order.Data;
            QueryOrderDetail = new Query().AddParams(Constans.OrderId, order.Data.Id);
        }

        protected async Task FilterProductName(FilteringEventArgs filtering)
        {
            try
            {
                DdlQuery.Queries.Params.Clear();
                DdlQuery.AddParams("ProductName", filtering.Text);
                await ddlProducts.RefreshDataAsync();
            }
            catch (Exception e)
            {
                await component.ShowErrorMessage(e.Message);
            }
        }

        protected void OnActionCompleteDDListProduct(ActionCompleteEventArgs<ProductReturnView> res)
        {
            if (selectIndex)
                SelectIndexDDLProducts(res.Result);
        }

        protected void OnActionBeginDDListProduct(ActionBeginEventArgs obj)
        {
            if (DdlQuery.Queries != null)
            {
                DdlQuery.Queries.Params.Clear();
                DdlQuery.AddParams(Constans.ProductIds, ExcludeProductIdsStr);
            }
        }
        protected async Task<string> ExcluseProductIds()
        {
            var res = await DmOrderDetail.ExecuteQueryAsync<OrderDetailReturnView>(
                new Query().AddParams(Constans.OrderId, SelectOrder.Id));
            var list = (List<OrderDetailReturnView>)res;
            var str = string.Join(',', list.Select(el => el.ProductID));

            return str;
        }

        public void CustomActionCompleteOrderDetail(ActionEventArgs<OrderDetailReturnView> obj)
        {
            if (obj.RequestType == Action.Save)
            {
                var date = obj.Data as OrderDetailReturnView;
                //await orderGrid.SetCustomValueToCell(date.Id, nameof(OrderReturnView.TotalSumm), date.TotalSumm);
                //await orderGrid.SetCustomRowSelectedById(date.Id.Value);
            }
        }

        public async Task OnActionBeginProductDetail(ActionEventArgs<OrderDetailReturnView> obj)
        {
            try
            {
                if (obj.RequestType == Action.Add || obj.RequestType == Action.BeginEdit)
                {
                    var orderDetail = (obj.EditContext.Model as OrderDetailReturnView);
                    if (obj.RequestType == Action.Add)
                        orderDetail.IsNew = true;
                    orderDetail.Id = SelectOrder.Id;
                    QueryOrderDetail = new Query();
                    QueryOrderDetail.AddParams(Constans.OrderId, SelectOrder.Id);
                }

                if (obj.RequestType == Action.Save)
                {
                    QueryOrderDetail = new Query();
                    QueryOrderDetail.AddParams(Constans.OrderId, SelectOrder.Id);
                }
                if (obj.RequestType == Action.Delete)
                {
                    // orderDeteilGrid.Query = new Query().AddParams("ProductId", SelectOrderDetail.ProductID);
                    //await orderDeteilGrid.DeleteRecordAsync("ProductId", SelectedOrderDetail);
                    header.Clear();
                    header.Add(Constans.ProductId, SelectOrderDetail.ProductID.ToString());
                }

            }
            catch (Exception e)
            {
                await component.ShowErrorMessage(e.Message);
            }
        }

        protected async Task OnBtnOkClick()
        {
            try
            {
                //await ddlProducts.RefreshDataAsync();

                // ddlProducts.Index=2;
                if (!selectIndex)
                    ddlProducts.SetIndex(2);
                else
                    SelectIndexDDLProducts(listProductDDl);
                await sfDialog.HideAsync();
                //var SelectProudct = products.SelectedProduct;
                // await ddlProducts.RefreshDataAsync();
                if (products.SelectedProduct != null)
                    selectIndex = true;
                orderDeteilGrid.CustomPreventRender(false);

                //SelectOrderDetail.ProductID = SelectProudct.Id;


                //ddlProducts.Value = SelectProudct.Id;

                //(editContextDetail.Model as OrderDetailReturnView).ProductID = SelectProudct.Id;
                //var field= editContextDetail.Field(nameof(OrderDetailReturnView.ProductID));
                //editContextDetail.NotifyFieldChanged(field);
            }
            catch (Exception e)
            {
                await component.ShowErrorMessage(e.Message);
            }
        }

        private bool selectIndex;
        private List<ProductReturnView> listProductDDl { get; set; }



        private void SelectIndexDDLProducts(IEnumerable<ProductReturnView> result)
        {
            if (result != null && result.Any())
            {
                var list = listProductDDl = result.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Id == products.SelectedProduct.Id)
                    {
                        ddlProducts.SetIndex(i);
                        break;
                    }
                }
            }
        }

        protected void OnCloseDDListProducts(PopupEventArgs t)
        {
            if (ddlProducts.Value != null)
            {
                var res = ddlProducts.GetDataByValue(ddlProducts.Value);
                orderDetail.UnitsInStock = res.UnitsInStock;
            }
        }
        protected void OnRowSelected(RowSelectingEventArgs<OrderDetailReturnView> order)
        {
            SelectOrderDetail = order.Data;
            if (string.IsNullOrEmpty(ExcludeProductIdsStr)) return;
            var list = ExcludeProductIdsStr.Split(',').ToList();
            list.Remove(SelectOrderDetail.ProductID.ToString());
            ExcludeProductIdsStr = string.Join(',', list);
        }
    }


}
