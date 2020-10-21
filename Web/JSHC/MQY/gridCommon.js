var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;
var gridManager = null;

function GridInitControl() {
    gridManager = $("#grid").ligerGrid({
        isScoll: false,
        pageSize: 20, width: '99%', height: '98%',
        pageSizeOptions: 20,
        enabledSort: false,
        usePager: true,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast
    });
}

function GridInitIDControl(id) {
    gridManager = $("#" + id).ligerGrid({
        isScoll: false,
        pageSize: 20, width: '99%', height: '98%',
        pageSizeOptions: 20,
        enabledSort: false,
        usePager: true,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast
    });
}

function GridCount(BLL, method, arr) {
    MaxCount = ReturnValue(BLL, method, arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
    MaxPage = MaxPage == 0 ? 1 : MaxPage; 
}

function GridLoad(BLL, Method, Para) {
    gridManager = $("#grid").ligerGrid({
        url: ActionUrl(BLL, Method, Para), pageSize: 20
    });
}

function GridLoadAll(BLL, Method, MethodCount, Para) {
    GridCount(BLL, MethodCount, Para);
    gridManager = $("#grid").ligerGrid({
        url: ActionUrl(BLL, Method, Para)
    });
}

function GridLoadIDAll(BLL, Method, MethodCount, Para, id) {
    GridCount(BLL, MethodCount, Para);
    gridManager = $("#" + id).ligerGrid({
        url: ActionUrl(BLL, Method, Para)
    });
}
function ActionUrl(bll, method, para) {
    var arrStr = para;
    ReplaceStr(arrStr);
    return "../../Ashx/GetPublicData.ashx?Data=" + method + "臡" + bll + "臡" + encodeURI(arrStr);
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

//首页
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