var Address = "../";
var gridManager = null;
var toptoolbarManager = null;
var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;

$(function () {
    InitControl();
    InitData();
});

//初始化控件
function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
             { text: '查询', id: "search", icon: 'search', click: onToFirst }
        ]
    });
    GetMenuRight(toptoolbarManager, "2016112311946");

    gridManager = $("#grid").ligerGrid({
        columns: [
                    { display: '平台名称', name: 'Name', minWidth: 150 },
                    { display: '客户名称', name: 'ODD_Name', minWidth: 150 },
                    { display: '座机号码', name: 'ODD_Phone', minWidth: 100 },
                    { display: '地址', name: 'ODD_Address', minWidth: 300 },
                    { display: '联系人', name: 'ODD_LinkMan', maxWidth: 80 },
                    { display: '联系电话', name: 'ODD_LinkPhone', minWidth: 100 },
                    { display: '是否计费', name: 'ODD_IsBill', maxWidth: 70 }               
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

function InitData() {
    Search();
}

//查询
function Search() {
    var arr = new Array();
    arr[0] = $("#Cust_Name").val();
    arr[1] = $("#Cust_Tel").val();
    arr[2] = $("#Cust_Phone").val();
    arr[3] = PageIndex;
    arr[4] = PageNum;
    
    MaxCount = ReturnValue("BLL_AllQuery", "GetValidDataCount", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetValidData";
    var BLL = "BLL_AllQuery";
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