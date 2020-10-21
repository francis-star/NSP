var Address = "../../"; //当前的html与Aspx中PubForm的文件深度
var StuffCode = GetQueryString("code");

$(function () {
    InitControl();
    Search();
});

function Search() {
    var arr = new Array();
    arr[0] = StuffCode;
    arr[1] = PageIndex;
    arr[2] = PageNum;

    GridLoadAll("BLL_OriginalData", "GetXFBOriginalDataDts", "GetXFBOriginalDataDtsCount", arr);
}

function OutExcel() {
    var arr = new Array();
    arr[0] = StuffCode;
    arr[1] = "XFB_OriginalDataDts";

    var data = ReturnValue("BLL_OrigDataImport", "DtToExcel", arr, Address);
    if (data == "true")
        $.ligerDialog.open({ url: "../../Aspx/ImportHelper.aspx" });
    else
        $.ligerDialog.error('当前内容为空,无法导出！');
}

function InitControl() {
    $("#toptoolbar").ligerToolBar({
        items: [
            { text: '导出', icon: 'up', click: OutExcel }
        ]
    });

    gridManager = $("#grid").ligerGrid({
        columns: [
            { display: '客户名称', name: 'ODD_Name', minWidth: 80 },
            { display: '联系电话', name: 'ODD_Phone', minWidth: 80 }
        ],
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
