var gridManager = null;
var toptoolbarManager = null;
var groupicon = "../../JS/lib/ligerUI/skins/icons/communication.gif";
var Address = "../../";
var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;
var UserName = "";

$(function () {
    UserName = ReturnValue("BLL_CallCenter", "GetUserName", "", Address);
    InitControl();
    Search();
});

//初始化控件
function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '查询', id: "btnsearch", icon: 'search', click: SearchData },
            { line: true },
            { text: '新增', id: 'btnAdd', icon: 'add', click: Add },
            { line: true },
            { text: '修改', id: 'btnModify', icon: 'modify', click: Modify },
            { line: true },
            { text: '删除', id: 'btnDelete', icon: 'delete', click: Delete }
        ]
    });

    $("#Inp_BeginTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 160 });
    $("#Inp_EndTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 160 });

    //初始化客户状态
    //var Data = eval(ReturnValue("BLL_CallCenter", "GetState", "", Address));
    var Data = eval("[{\"Cset_Name\":\"待审\",\"Cset_Code\":\"待审\"},{\"Cset_Name\":\"已审\",\"Cset_Code\":\"已审\"},{\"Cset_Name\":\"退订\",\"Cset_Code\":\"退订\"},{\"Cset_Name\":\"退费\",\"Cset_Code\":\"退费\"},{\"Cset_Name\":\"退回\",\"Cset_Code\":\"退回\"}]");
    $("#CustomerState").ligerComboBox({
        width: 160,
        selectBoxWidth: 160,
        data: Data,
        textField: "Cset_Name",
        valueField: "Cset_Code",
        initValue: "待审;退回",
        required: "true",
        isMultiSelect: true,
        allowInput: "false"
    });

    gridManager = $("#grid").ligerGrid({
        columns: [
            {
                display: '', width: 30, render: function (rowdata, rowindex, value) {
                    var s = "";
                    if (rowdata.Cust_State == "退回") {
                        s = "<a href='#' title='" + rowdata.Cust_ReturnContent + "' id='tip' onmouseover='gg(this);' ><img height= '24'  width='24' style='cursor:pointer' src='../../Images/jg.png'  /></a>";
                    }
                    return s;
                }
            },
            { display: '客户名称', name: 'Cust_Name', width: 180 },
            { display: '联系电话', name: 'Cust_Phone', minWidth: 90 },
            { display: '计费号码', name: 'Cust_BillNumber', minWidth: 90 },
            { display: '客户状态', name: 'Cust_State', minWidth: 90 },
            { display: '退回人', name: 'Cust_ReturnBackMan', minWidth: 90 },
            { display: '开通时间', name: 'Cust_OpenDate', minWidth: 90 },
            {
                display: '地区', minWidth: 90, render: function (rowdata, rowindex, value) {
                    var s = "";
                    var id = "tip" + rowdata.Cust_Code;
                    s = "<span>" + rowdata.Cust_Area + "</span>"
                    return s;
                }
            }
        ],
        pageSize: PageNum, width: '98%', height: '98%',
        pageSizeOptions: PageNum,
        enabledSort: false,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast
    });

    GetMenuRight(toptoolbarManager, "2019062711943");
}

function SearchData() {
    onToFirst();
}

function Search() {
    var arr = new Array();
    arr[0] = $("#CustName").val();
    arr[1] = $("#CustPhone1").val();
    arr[2] = "";// $("#Custlinkman").val();
    arr[3] = "";//$("#CustlinkPhone").val();
    arr[4] = $("#Cust_BillNumber").val();
    arr[5] = $("#CustomerState").val();
    arr[6] = ($("#Inp_BeginTime").val() == "" ? "" : $("#Inp_BeginTime").val() + " 00:00:00");
    arr[7] = ($("#Inp_EndTime").val() == "" ? "" : $("#Inp_EndTime").val() + " 23:59:59");
    arr[8] = PageIndex;
    arr[9] = PageNum;
    arr[10] = UserName;

    MaxCount = ReturnValue("BLL_CallCenter", "GetXFBDataCount", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));

    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetXFBCallCenterData";
    var BLL = "BLL_CallCenter";
    gridManager = $("#grid").ligerGrid({
        url: "../../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
    });
    return false;
}

function Add() {
    top.layer.open({
        type: 2,
        title: '添加客户单',
        shadeClose: false,
        shade: 0.3,
        area: ['70%', '80%'],
        content: '../../Form/XFB/frm_CallCenterDts.htm?Code=',//iframe的url
        end: function () {
            Search();
        }
    });
}

function Modify() {
    var row = gridManager.getSelected();
    if (row) {
        if (row.Cust_State == "待审" || row.Cust_State == "退回") {
            top.layer.open({
                type: 2,
                title: '修改客户单',
                shadeClose: false,
                shade: 0.3,
                area: ['70%', '80%'],
                content: '../../Form/XFB/frm_CallCenterDts.htm?fromPage=CallCenter_A&Code=' + row.Cust_Code, //iframe的url
                end: function () {
                    Search();
                }
            });
        } else {
            $.ligerDialog.warn("只能修改待审和退回的数据!");
        }
    }
    else {
        $.ligerDialog.warn("请选择一条记录!");
        return;
    }
}

function Delete() {
    var row = gridManager.getSelected();
    if (row) {
        if (row.Cust_State != "待审" && row.Cust_State != "退回") {
            $.ligerDialog.error("只能删除待审或退回的数据!");
            return;
        }
        $.ligerDialog.confirm('确定删除？', function (yes) {
            if (yes) {
                var arr = new Array();
                arr[0] = row.Cust_Code;
                var Data = ReturnValue("BLL_CallCenter", "DeleteXFBCallCenterData", arr, Address);
                if (Data == "true") {
                    $.ligerDialog.success("删除成功!");
                    Search();
                } else {
                    $.ligerDialog.error("删除失败!");
                }
            }
        });
    }
    else {
        $.ligerDialog.warn("请选择一条记录!");
        return;
    }
}

//针对一个传参时，如果出现参数为空时，自动补充一个空参数，且中文逗号取代英文逗号
function ReplaceStr(para) {
    //针对一个传参时，如果出现参数为空时，自动补充一个空参数
    if (para.length == 1 && para[0] == "")
        para[1] = "";
    if (para.length > 0) {
        for (var i = 0; i < para.length; i++) {
            if (para[i] != undefined && isNaN(para[i])) {
                para[i] = para[i].replace(/,/g, '，');
            }
        }
    }
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


function gg(t) {
    $(t).poshytip({
        className: 'tip-yellow',
        alignTo: 'target',
        alignX: 'right',
        alignY: 'center',
        offsetX: 5
    });
}