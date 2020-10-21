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
                    { text: '添加', id: 'btnAdd', icon: 'add', click: Add},
                    { line: true },
                    { text: '删除', id: 'btnDelete', icon: 'delete', click: DeleteRedBlackList },
        ]
    });
    GetMenuRight(toptoolbarManager, "2016112311937");

    //$("#Province").ligerComboBox({
    //    data: "",
    //    textField: "LS_Name",
    //    valueField: "LS_Code",
    //    valueFieldID: "value5",
    //    width: 135,
    //    cancelable: false,
    //    isMultiSelect: false
    //});

    //$("#City").ligerComboBox({
    //    data: "",
    //    textField: "LS_Name",
    //    valueField: "LS_Code",
    //    valueFieldID: "value6",
    //    width: 135,
    //    cancelable: false,
    //    isMultiSelect: false
    //});
    //GetArea1();

    gridManager = $("#grid").ligerGrid({
        columns: [
                   { display: '黑名单号码', name: 'BL_Phone', minWidth: 90 },
                   { display: '备注信息', name: 'BL_Comment', minWidth: 90 }
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
        cancelable: false,
        onSelected: function (newvalue) {
            if (newvalue != null && newvalue != "") {
                $("#Inp_City").val("");
                $("#value6").val("");

                GetArea2(newvalue);
            } else {
                $("#City").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value6",
                    width: 135,
                    cancelable: false,
                    isMultiSelect: false
                });
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
        width: 135,
        isMultiSelect: false,
        cancelable: false
    });
}

function InitData() {
    Search();
}

function Add() {
    top.layer.open({
        type: 2,
        title: '添加黑名单',
        shadeClose: false,
        shade: 0.3,
        area: ['35%', '50%'],
        content: '../Form/frm_BlackListDts.htm?', //iframe的url
        end: function () {
            Search();
        }
    });
}

function Search() {
    var arr = new Array();
    //var Province = $("#Province").val();
    //if ($("#Province").val() == "全国") {
    //    Province = "";
    //}
    //arr[0] = Province;
    //var City = $("#City").val();
    //if ($("#City").val() == "全部") {
    //    City = "";
    //}
    arr[1] = "";
    arr[2] = $("#BL_Phone").val();
    arr[3] = $("#BL_Comment").val();
    arr[4] = PageIndex;
    arr[5] = PageNum;



    MaxCount = ReturnValue("BLL_BlackList", "GetNum", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));

    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetBlackList";
    var BLL = "BLL_BlackList";
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
                str += this.BL_Code;
            });

            var arr = new Array();
            arr[0] = str;
            ReturnMethod("BLL_BlackList", "DelBlackList", arr, Address, "DeleteSucess");
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

