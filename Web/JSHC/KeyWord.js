var gridManager;
var Address = "../";
var toptoolbarManager = null;
var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;

$(function () {
    InitControl();
    InitData();
});

function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '查询', id: 'btnSearch', icon: 'search', click: SearchData },
            { line: true },
            { text: '添加', id: 'btnAdd', icon: 'add', click: Add },
            { line: true },
            { text: '删除', id: 'btnDelete', icon: 'delete', click: DeleteRedBlackList },
        ]
    });
    GetMenuRight(toptoolbarManager, "2016112311938");

    $("#Province").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value5",
        width: 135,
        cancelable: false
    });

    GetArea1();

    gridManager = $("#grid").ligerGrid({
        columns: [
            { display: '关键字', name: 'KM_Name', minWidth: 90 },
            {
                display: '地区', name: 'KM_ProvinceName' 
            },
            { display: '录入人', name: 'JoinMan', minWidth: 90 },
            {
                display: '业务', name: 'KM_PlatForm', minWidth: 90, render: function (rowdata, rowindex, value) {
                    var s = "";
                    var n = "";
                    s = rowdata.KM_PlatForm;
                    if (s == "1") {
                        n = "维权通";
                    } else if (s == "2") {
                        n = "民企云";
                    } else if (s == "3") {
                        n = "诚信通";
                    }
                    else {
                        n = "实时保";
                    }
                    return n;
                }
            },
            {
                display: '危险程度', name: 'KM_DangerousDegree', minWidth: 90, render: function (rowdata, rowindex, value) {
                    var s = "";
                    var n = "";
                    s = rowdata.KM_DangerousDegree;
                    if (s == "0") {
                        n = "低";
                    } else {
                        n = "高";
                    }
                    return n;
                }
            }
        ],
        pageSize: 20, width: '98%', height: '98%',
        pageSizeOptions: PageNum,
        enabledSort: false,
        usePager: true,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast
    });

    $("#PlatForm").ligerComboBox({
        width: 150,
        data: [
            { text: '维权通', id: '1' },
            { text: '民企云', id: '2' },
            { text: '诚信通', id: '3' },
            { text: '实时保', id: '5' }
        ]
    });

    $("#isDanger").ligerComboBox({
        width: 150,
        data: [
            { text: '低', id: '0' },
            { text: '高', id: '1' }
        ]
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
        value: Data[0].SA_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false
    });
}
 
function Add() {
    top.layer.open({
        type: 2,
        title: '添加关键字',
        shadeClose: false,
        shade: 0.3,
        area: ['35%', '45%'],
        content: '../Form/frm_KeyWordDts.htm?', //iframe的url
        end: function () {
            Search();
        }
    });
}

function Search() {
    var arr = new Array();
    var Province = $("#Province").val();
    if ($("#Province").val() == "全国") {
        Province = "";
    }
    arr[0] = Province; 
    arr[1] = $("#KM_Name").val();
    arr[2] = PageIndex;
    arr[3] = PageNum;
    if ($("#PlatForm").val() == "维权通") {
        arr[4] = 1;
    } else if ($("#PlatForm").val() == "民企云") {
        arr[4] = 2;
    } else if ($("#PlatForm").val() == "诚信通") {
        arr[4] = 3;
    } else if ($("#PlatForm").val() == "实时保") {
        arr[4] = 5;
    } else {
        arr[4] = "";
    }
    if ($("#isDanger").val() == "低") {
        arr[5] = 0;
    } else if ($("#isDanger").val() == "高") {
        arr[5] = 1;
    } else {
        arr[5] = "";
    }
    MaxCount = ReturnValue("BLL_KeyWord", "GetNum", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));

    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "SearchWord";
    var BLL = "BLL_KeyWord";
    gridManager = $("#grid").ligerGrid({
        url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
    });


}

function DeleteRedBlackList() {
    var rows = gridManager.getCheckedRows();
    if (rows == null || rows == "") {
        $.ligerDialog.warn('请选择删除行！');
        return false;
    }

    $.ligerDialog.confirm('删除后将无法恢复！确认要删除该数据？', function (yes) {
        if (yes) {
            var str = "";
            $(rows).each(function () {
                str += this.KM_Code;
            });

            var arr = new Array();
            arr[0] = str;
            ReturnMethod("BLL_KeyWord", "DelKeyManager", arr, Address, "DeleteSucess");
        }
    });
}

var CustomMethod = {
    DeleteSucess: function (text) {
        if (text == 'true') {
            $.ligerDialog.success('删除成功！');
        }
        else {
            $.ligerDialog.error('删除失败！');
        }

        InitData();
    }
}
function SearchData() {
    onToFirst();
}

function InitData() {
    Search();
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