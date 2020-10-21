var gridManager = null;
var toptoolbarManager = null;
var groupicon = "../../JS/lib/ligerUI/skins/icons/communication.gif";
var Address = "../../";
var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;
var combUser;

$(function () {
    InitControl();
    Search();
});

//初始化控件
function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '查询', id: "btnsearch", icon: 'search', click: SearchData },
            { line: true },
            { text: '批量计费', id: 'btnBatch', icon: 'add', click: Add },
            { line: true },
            { text: '撤销任务', id: 'btnCancel', icon: 'modify', click: Modify },
            { line: true },
            { text: '删除任务', id: 'btnDelete', icon: 'delete', click: Delete },
            { line: true },
            { text: '查看明细', id: 'btnView', icon: 'search2', click: View }
        ]
    });

    $("#Inp_BeginTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 160 });
    $("#Inp_EndTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 160 });

    //初始化客户状态 
    var Data = eval("[{\"Cset_Name\":\"全部\",\"Cset_Code\":\"全部\"},{\"Cset_Name\":\"已执行\",\"Cset_Code\":\"已执行\"},{\"Cset_Name\":\"已撤销\",\"Cset_Code\":\"已撤销\"}]");
    $("#txtState").ligerComboBox({
        width: 160,
        selectBoxWidth: 160,
        data: Data,
        textField: "Cset_Name",
        valueField: "Cset_Code",
        initValue: "全部",
        required: "true",
        isMultiSelect: false,
        allowInput: "false"
    });

    var userData = eval("(" + ReturnValue("BLL_UserSet", "GetUserList", "", Address) + ")");
    combUser = $("#txtOperateMan").ligerComboBox({
        width: 160,
        selectBoxWidth: 160,
        data: userData,
        textField: "User_Name",
        valueField: "User_Code",
        isMultiSelect: true,
        allowInput: "false"
    });

    gridManager = $("#grid").ligerGrid({
        columns: [
            { display: '当前状态', name: 'StateName', width: 90 },
            { display: '导入文件名', name: 'CD_FileName', minWidth: 150 },
            { display: '操作人', name: 'UpdateUserName' },
            { display: '操作时间', name: 'JoinDate', minWidth: 90 },
            { display: '备注', name: 'CD_Remark'}
        ],
        pageSize: PageNum, width: '98%', height: '98%',
        pageSizeOptions: PageNum,
        enabledSort: false,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast
    });
    GetMenuRight(toptoolbarManager, "2019102211946");
}

function SearchData() {
    onToFirst();
}

function Search() { 
    var arr = new Array();
    arr[0] = $("#txtState").val() == "全部" ? "" : $("#txtState").val();
    arr[1] = combUser.getValue();
    arr[2] = ($("#Inp_BeginTime").val() == "" ? "" : $("#Inp_BeginTime").val() + " 00:00:00");
    arr[3] = ($("#Inp_EndTime").val() == "" ? "" : $("#Inp_EndTime").val() + " 23:59:59");
    arr[4] = PageIndex;
    arr[5] = PageNum;

    MaxCount = ReturnValue("BLL_CallCenter", "GetSSBDealChargeDataCount", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));

    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetSSBDealChargeData";
    var BLL = "BLL_CallCenter";
    gridManager = $("#grid").ligerGrid({
        url: "../../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
    });
    return false;
}

//批量计费
function Add() {
    top.layer.open({
        type: 2,
        title: '批量计费',
        shadeClose: false,
        shade: 0.3,
        area: ['369px', '420px'],
        content: '../../Form/SSB/frm_ImportBatachData.htm',//iframe的url
        end: function () {
            Search();
        }
    });
}

//撤销任务
function Modify() {
    var row = gridManager.getSelected();
    if (row) {
        $.ligerDialog.confirm('撤销后，此次批量任务的相关操作将会还原，请谨慎处理。', function (yes) {
            if (yes) {
                var arr = new Array();
                arr[0] = row.CD_Code;
                var Data = ReturnValue("BLL_CallCenter", "CancelSSBChargeData", arr, Address);
                if (Data.indexOf("true") > -1) {
                    $.ligerDialog.success("撤销成功！");
                    Search();
                } else {
                    $.ligerDialog.error(Data.split("|")[1]);
                }
            }
        });
    }
    else {
        $.ligerDialog.warn("请选择一条记录!");
        return;
    }
}

function Delete() {
    var row = gridManager.getSelected();
    if (row) { 
        $.ligerDialog.confirm('删除后，数据将无法恢复，确定要执行删除操作吗？', function (yes) {
            if (yes) {
                var arr = new Array();
                arr[0] = row.CD_Code;
                var Data = ReturnValue("BLL_CallCenter", "DeleteSSBChargeData", arr, Address);
                if (Data.indexOf("true") > -1) {
                    $.ligerDialog.success("已删除！");
                    Search();
                } else {
                    $.ligerDialog.error(Data.split("|")[1]);
                }
            }
        });
    }
    else {
        $.ligerDialog.warn("请选择一条记录!");
        return;
    }
}

//查看明细
function View() {
    var row = gridManager.getSelected();
    if (row) {
        top.layer.open({
            type: 2,
            title: '查看明细',
            shadeClose: false,
            shade: 0.3,
            area: ['70%', '80%'],
            content: '../../Form/SSB/frm_ViewDealChargeDts.htm?Code=' + row.CD_Code, //iframe的url
            end: function () {
                Search();
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