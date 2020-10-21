var Address = "../";
var gridManager = null;
var toptoolbarManager = null;
var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;

$(function () {
    InitControl();
    GetWHUser();
    GetArea1();
    InitData();
});

//初始化控件
function InitControl() {
    $("#Un_OrderStart").ligerDateEditor({ format: "yyyy-MM-dd", width: 110 });
    $("#Un_OrderEnd").ligerDateEditor({ format: "yyyy-MM-dd", width: 110 });
    $("#Cust_OutDateStart").ligerDateEditor({ format: "yyyy-MM-dd", width: 110 });
    $("#Cust_OutDateEnd").ligerDateEditor({ format: "yyyy-MM-dd", width: 110 });
    $("#Cust_OpenDateStart").ligerDateEditor({ format: "yyyy-MM-dd", width: 110 });
    $("#Cust_OpenDateEnd").ligerDateEditor({ format: "yyyy-MM-dd", width: 110 });

    var StateData = eval("[{\"Cset_Name\":\"已审\",\"Cset_Code\":\"已审\"},{\"Cset_Name\":\"退订\",\"Cset_Code\":\"退订\"},{\"Cset_Name\":\"退费\",\"Cset_Code\":\"退费\"}]");
    //var StateData = eval(ReturnValue("BLL_CustomerService", "GetState", "", Address));
    ligerComboBoxs = $("#Cust_State").ligerComboBox({
        width: 162,
        selectBoxWidth: 162,
        selectBoxHeight: 120,
        //textField: 'CSet_Name', valueField: 'CSet_Code',
        textField: "Cset_Name",
        valueField: "Cset_Code",
        isMultiSelect: true,
        initValue: "已审",
        data: StateData
    });

    $("#Cust_IsBill").ligerComboBox({
        width: 160,
        selectBoxWidth: 160,
        initValue: "是",
        data: [
            { text: '是', id: '是' },
            { text: '否', id: '否' }
        ]
    });

    $("#Province").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value5",
        width: 110,
        cancelable: false,
        isMultiSelect: false
    });

    $("#City").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value6",
        width: 110,
        cancelable: false,
        isMultiSelect: false
    });

    var Data = eval(ReturnValue("BLL_CustomerReturns", "GetUserPlace", "", Address));
     
    $("#tbPlace").ligerComboBox({
        data: Data,
        textField: "User_Place",
        valueField: "User_Place",
        valueFieldID: "value1",
        value: Data[0].User_Place,
        width: 135,
        isMultiSelect: false,
        cancelable: false
    });

    //var strPlace = [{ 'text': '全部' }, { 'text': '本部' }, { 'text': '苏州' }, { 'text': '徐庄' }, { 'text': '山东' }];
    //$("#tbPlace").ligerComboBox({
    //    data: strPlace,
    //    textField: 'text', width: 180
    //});
    liger.get("tbPlace").setText("全部");

    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '查询', id: "search", icon: 'search', click: onToFirst },
            { line: true },
            {
                text: '处理', id: 'btnDeal', icon: 'config', click: function () {
                    var row = gridManager.getSelected();
                    if (row) {
                        top.layer.open({
                            type: 2,
                            title: '处理信息',
                            shadeClose: false,
                            shade: 0.3,
                            area: ['90%', '90%'],
                            content: '../Form/frm_CustomerReturnsDts.htm?Code=' + row.Cust_Code, //iframe的url
                            end: function () {
                                Search();
                            }
                        });
                    }
                    else {
                        $.ligerDialog.warn('请选择要处理的客户！'); return;
                    }
                }
            },
            { line: true },
            {
                text: '编辑', id: 'btnModify', icon: 'modify', click: function () {
                    var row = gridManager.getSelected();
                    if (row) {
                        top.layer.open({
                            type: 2,
                            title: '编辑信息',
                            shadeClose: false,
                            shade: 0.3,
                            area: ['90%', '90%'],
                            content: '../Form/frm_CallCenterDts.htm?fromPage=CustomerReturns_A&Code=' + row.Cust_Code, //iframe的url
                            end: function () {
                                Search();
                            }
                        });
                    }
                    else {
                        $.ligerDialog.warn('请选择要编辑的客户！'); return;
                    }
                }
            },
            { line: true },
            {
                text: '查看', id: "btnView", icon: 'view', click: function () {
                    var row = gridManager.getSelected();
                    if (row) {
                        top.layer.open({
                            type: 2,
                            title: '查看',
                            shadeClose: false,
                            shade: 0.3,
                            area: ['90%', '90%'],
                            content: '../Form/frm_CustomerReturnscheck.htm?Code=' + row.Cust_Code, //iframe的url
                            end: function () {
                                Search();
                            }
                        });
                    }
                    else {
                        $.ligerDialog.warn('请选择要查看的客户！'); return;
                    }
                }
            },
            { line: true },
            {
                text: '导出', id: 'btnExport', icon: 'up', click: OutExcel
            }
            //,
            //{ line: true },
            //{
            //    text: '异常退回', id: "btnBack", icon: 'delete', click: function () {
            //        var row = gridManager.getSelected();
            //        if (row) {
            //            if (row.Cust_State != '已审') {
            //                $.ligerDialog.warn('请选择已审状态的客户！'); return;
            //            }
            //            else {
            //                $.ligerDialog.confirm('确定将当前已审核的客户单退回么？', function (yes) {
            //                    if (yes) {
            //                        var arr = new Array();
            //                        arr[0] = row.Cust_Code;
            //                        var data = ReturnValue("BLL_CustomerReturns", "BackCXTCustomer", arr, Address, "");
            //                        if (data == "true")
            //                            $.ligerDialog.success('异常退回成功！');
            //                        Search();
            //                    }
            //                });
            //            }
            //        }
            //        else {
            //            $.ligerDialog.warn('请选择要查看的客户！'); return;
            //        }
            //    }
            //}

        ]
    });
    GetMenuRight(toptoolbarManager, "2016112311929");

    gridManager = $("#grid").ligerGrid({
        columns: [
            { display: '客户名称', name: 'Cust_Name', minWidth: 150 },
            { display: '座机号码', name: 'Cust_Phone', minWidth: 100 },
            { display: '计费号码', name: 'Cust_BillNumber', minWidth: 100 },
            { display: '联系人', name: 'Cust_Linkman', maxWidth: 80 },
            { display: '联系电话', name: 'Cust_LinkPhone', minWidth: 100 },
            { display: '外呼人员', name: 'Cust_WH_UserName', maxWidth: 80 },
            { display: '工作地点', name: 'User_Place', maxWidth: 80 },
            { display: '客户状态', name: 'Cust_State', maxWidth: 70 }, 
            { display: '录音编码', name: 'Cust_KFVoice', minWidth: 150 },
            { display: '开通时间', name: 'Cust_OpenDate', minWidth: 90 },
            { display: '审核人', name: 'Cust_PassMan', minWidth: 90 },
            { display: '退订时间', name: 'Cust_UnOrder', minWidth: 90 },
            { display: '退订人', name: 'Cust_DealPerson', minWidth: 90 },
            { display: '退费时间', name: 'Cust_OutDate', minWidth: 90 },
            {
                display: '地区', minWidth: 135, render: function (rowdata, rowindex, value) {
                    var s = "";
                    var id = "tip" + rowdata.Cust_Code;
                    s = "<span  title='" + rowdata.Cust_Address + "' id='" + id + "' style='cursor:pointer;' onmouseover='gg(this);'>" + rowdata.Cust_Area + "</span>"
                    return s;
                }
            }
        ],
        pageSize: 20, width: '98.8%', height: '98%',
        pageSizeOptions: PageNum,
        enabledSort: false,
        usePager: true,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast
    });

}

//省下拉框
function GetArea1() {
    var arr = new Array();
    arr[0] = '0';
    arr[1] = 'D0000001';
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"全国\"}]")[0]);//追加全国选项
    }
    $("#Province").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value5",
        width: 110,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            if (newvalue != null && newvalue != "") {
                $("#City").val("");
                $("#value6").val("");

                GetArea2(newvalue);
            } else {
                $("#City").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value6",
                    width: 110,
                    cancelable: false,
                    isMultiSelect: false
                });
                $("#City").val("");
                $("#value6").val("");
            }
        }
    });
}

//市下拉框
function GetArea2(DataCode) {
    var arr = new Array();
    arr[0] = '1';
    arr[1] = DataCode;
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"全部\"}]")[0]);//追加全部选项
    }
    $("#City").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value6",
        value: Data[0].SA_Code,
        width: 110,
        isMultiSelect: false,
        cancelable: false
    });
}

function GetWHUser() {
    var data = eval("(" + ReturnValue("BLL_PublicInfo", "GetWHUser", "", Address) + ")");
    $("#Cust_WH_UserName").ligerComboBox({
        data: data,
        textField: "Cust_WH_UserName",
        valueField: "Cust_WH_UserName",
        autocomplete: true,
        keySupport: true,
        width: 162,
        //isMultiSelect: true,
    });
}
function gg(t) {
    $(t).poshytip({
        className: 'tip-yellow',
        alignTo: 'target',
        alignX: 'right',
        alignY: 'center',
        offsetX: 5
    });
}

function InitData() {
    Search();
}

//导出
function OutExcel() {
    var arr = new Array();
    arr[0] = $("#Cust_Name").val();
    arr[1] = $("#Cust_Linkman").val();
    arr[2] = $("#Cust_LinkPhone").val();
    arr[3] = $("#Province").ligerComboBox().getValue() == "D0000001" ? "" : $("#Province").ligerComboBox().getValue();
    arr[4] = $("#City").ligerComboBox().getValue();
    arr[5] = $("#Cust_State").val();
    arr[6] = $("#Cust_BillNumber").val();
    arr[7] = $("#Cust_IsBill").val();
    arr[8] = $("#Cust_WH_UserName").val();
    arr[9] = $("#Cust_OpenDateStart").val();
    arr[10] = $("#Cust_OpenDateEnd").val();
    arr[11] = $("#Un_OrderStart").val();
    arr[12] = $("#Un_OrderEnd").val();
    arr[13] = $("#Cust_OutDateStart").val();
    arr[14] = $("#Cust_OutDateEnd").val();
    arr[15] = $("#tbPlace").val();
    var data = ReturnValue("BLL_CustomerReturns", "DtCTXToExcel", arr, Address);
    if (data == "true")
        $.ligerDialog.open({ url: "../Aspx/ImportHelper.aspx" });
    else
        $.ligerDialog.error('当前内容为空,无法导出！');
}

//查询
function Search() {
    var arr = new Array();
    arr[0] = $("#Cust_Name").val();
    arr[1] = $("#Cust_Linkman").val();
    arr[2] = $("#Cust_LinkPhone").val();
    arr[3] = $("#Province").ligerComboBox().getValue() == "D0000001" ? "" : $("#Province").ligerComboBox().getValue();
    arr[4] = $("#City").ligerComboBox().getValue();
    arr[5] = $("#Cust_State").val();
    arr[6] = $("#Cust_BillNumber").val();
    arr[7] = $("#Cust_IsBill").val();
    arr[8] = $("#Cust_WH_UserName").val();
    arr[9] = $("#Cust_OpenDateStart").val();
    arr[10] = $("#Cust_OpenDateEnd").val();
    arr[11] = $("#Un_OrderStart").val();
    arr[12] = $("#Un_OrderEnd").val();
    arr[13] = $("#Cust_OutDateStart").val();
    arr[14] = $("#Cust_OutDateEnd").val();
    arr[15] = PageIndex;
    arr[16] = PageNum;
    arr[17] = $("#tbPlace").ligerComboBox().getValue();


    MaxCount = ReturnValue("BLL_CustomerReturns", "GetNum", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));

    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetCust";
    var BLL = "BLL_CustomerReturns";
    gridManager = $("#grid").ligerGrid({
        url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
    });


}

function onToFirst() {
    PageIndex = 1;
    Search();
    return false;
}
//末页
function onToLast() {
    PageIndex = MaxPage;
    Search();
    return false;
}
//上一页
function onToPrev() {
    if (PageIndex - 1 > 0) {
        PageIndex = PageIndex - 1;
    } else {
        PageIndex = 1;
    }
    Search();
    return false;
}
//下一页
function onToNext() {
    if (PageIndex + 1 < MaxPage) {
        PageIndex = PageIndex + 1;
    } else {
        PageIndex = MaxPage;
    }
    Search();
    return false;
}

