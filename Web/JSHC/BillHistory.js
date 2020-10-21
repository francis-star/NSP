var Address = "../";
var gridManager = null;
var toptoolbarManager = null;
var PageIndex = 1;
var PageNum = 8;
var MaxPage = 0;
var MaxCount = 0;
var Code = GetQueryString("Code"); 

$(function () {
    InitControl();
    InitData();
});

function InitControl() {
    gridManager = $("#grid").ligerGrid({
        columns: [ 
            { display: '修改人', name: 'BH_User_Name', minWidth: 80 },
            { display: '修改时间', name: 'BH_Time', minWidth: 100 },
            { display: '修改了', name: 'BH_Content', minWidth: 30 },
            {
                display: '修改原因', name: 'BH_Remark', minWidth: 100, render: function (rowdata, rowindex, value) {
                    var h = "<div title='" + rowdata.BH_Remark + "'>" + rowdata.BH_Remark + "</div>";
                     
                    return h;
                }
            }
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

function InitData() {
    Search();
}

function Search() {
    var arr = new Array();
    arr[0] = Code;             //修改编码
    arr[1] = PageIndex;
    arr[2] = PageNum;

    MaxCount = ReturnValue("BLL_BillHistory", "GetBillHistoryCount", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));

    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetBillHistoryInfo";
    var BLL = "BLL_BillHistory";
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