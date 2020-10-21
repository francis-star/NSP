var Address = "../";
var gridManager = null;
var toptoolbarManager = null;
var PageIndex = 1;
var PageNum = 20;
var MaxPage = 0;
var MaxCount = 0;

$(function () {
    InitControl();
    GetArea1();
    InitData();
});


function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
                   { text: '查询', id: 'btnSearch', icon: 'search', click: SearchData },
                   { line: true },
                   { text: '添加', id: 'btnAdd', icon: 'add', click: Add },
                   { line: true },
                   { text: '删除', id: 'btnDelete', icon: 'delete', click: DeleteNoManager },
                   { line: true }
        ]
    });
    GetMenuRight(toptoolbarManager, "2016112311939");

    $("#Province").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value5",
        width: 145,
        cancelable: false,
        isMultiSelect: false
    });

    $("#City").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value6",
        width: 145,
        cancelable: false,
        isMultiSelect: false
    });

    $("#NmType").ligerComboBox({
        width: 150,
        data: [
                  { text: '移动', id: '1' },
                  { text: '电信', id: '2' },
                  { text: '联通', id: '3' }

        ]
    });

    gridManager = $("#grid").ligerGrid({
        columns: [ 
                    { display: '号段', name: 'NM_Phone', minWidth: 90 },
                    { display: '适用范围', name: 'AreaName', minWidth: 120 }, 
                    { display: '运营商', name: 'NM_Type', minWidth: 120 }
        ],
        pageSize: PageNum, width: '99%', height: '98%',
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
        value: Data[0].SA_Code,
        width: 145,
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
                    width: 145,
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
        width: 145,
        isMultiSelect: false,
        cancelable: false
    });
}

//初始化加载数据
function InitData() {
    Search();
}
//查询
function Search() {
    var arr = new Array();
    arr[0] = $("#Province").ligerComboBox().getValue() == "D0000001" ? "" : $("#Province").ligerComboBox().getValue();
    arr[1] = $("#City").ligerComboBox().getValue();
    arr[2] = $("#NmType").ligerComboBox().getText();
    arr[3] = $("#NmPhone").val();
    arr[4] = PageIndex;
    arr[5] = PageNum;

    MaxCount = ReturnValue("BLL_NoManager", "GetNoMangerCount", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));

    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetNoMangerInfo";
    var BLL = "BLL_NoManager";
    gridManager = $("#grid").ligerGrid({
        url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
    });
}
// 添加
function Add() {
    top.layer.open({
        type: 2,
        title: '添加号段',
        shadeClose: false,
        shade: 0.3,
        area: ['35%', '45%'],
        content: '../Form/frm_AddNoManager.htm', //iframe的url
        end: function () {
            Search();
        }
    });
}

// 删除
function DeleteNoManager() {
    var row = gridManager.getSelected();
    if (!row) {
        $.ligerDialog.warn('请选择删除的行！');
        return;
    }

    $.ligerDialog.confirm('确定删除该数据？', function (yes) {
        if (yes) {  
            var arr = new Array();
            arr[0] = row.NM_Code;
            var result = ReturnValue("BLL_NoManager", "DeleteNoManager", arr, Address);
            if (result == "true") {
                layer.msg("删除成功!");
                Search();
            } else {
                layer.msg("删除失败!");
            }
        }
    });
}

function SearchData() {
    onToFirst();
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