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
function checkit(str)
{
    layer.alert(str);
}
//初始化控件
function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
             { text: '查询', id: "search", icon: 'search', click: onToFirst },
            { line: true }
        ]
    });
    GetMenuRight(toptoolbarManager, "2016112311946");

    gridManager = $("#grid").ligerGrid({
        columns: [
                    { display: '数据来源', name: 'Sheet_Src', minWidth: 50 },
                    { display: '被投诉方名称', name: 'Cust_Name', minWidth: 150 },
                    { display: '被投诉方地址', name: 'Cust_Address', minWidth: 100 },
                    { display: '被投诉方电话', name: 'Cust_Phone', minWidth: 100 },
                    { display: '消费者姓名', name: 'Appe_UserName', maxWidth: 80 },
                    { display: '消费者电话', name: 'Appe_UserPhone', minWidth: 100 },
                    {
                        display: '投诉内容', name: 'Appe_ShopAsk', maxWidth: 200, render: function (rowdata, rowindex, value) {
                            var s = "";
                            s = "<a title='" + rowdata.Appe_ShopAsk + "' onclick=checkit($(this).attr('title')); style='cursor:pointer;color:blue' >详情</a>";
                            return s;
                        }
                    },
                    { display: '登记时间', name: 'JoinDate', maxWidth: 100 },

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
    arr[1] = $("#Cust_Phone").val();
    arr[2] = PageIndex;
    arr[3] = PageNum;


    MaxCount = ReturnValue("BLL_AllQuery", "GetTSSheetDataCount", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetTSSheetData";
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