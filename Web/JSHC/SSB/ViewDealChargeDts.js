var Address = "../../"; //当前的html与Aspx中PubForm的文件深度
var dealOrgCode = GetQueryString("code"); 

$(function () {
    InitControl();
    Search();
});

function Search() {
    var arr = new Array();
    arr[0] = dealOrgCode;
    arr[1] = PageIndex;
    arr[2] = PageNum;

    GridLoadAll("BLL_CallCenter", "GetSSB_ViewChargeData", "GetSSB_ViewChargeDataCount", arr);
}

function OutExcel() {
    var arr = new Array();
    arr[0] = dealOrgCode;
    arr[1] = "SSB_ViewChargeData"; 

    var data = ReturnValue("BLL_OrigDataImport", "DtToExcel", arr, Address);
    if (data == "true")
        $.ligerDialog.open({ url: "../../Aspx/ImportHelper.aspx" });
    else
        $.ligerDialog.error('当前内容为空,无法导出！');
}

function InitControl() {
    $("#toptoolbar").ligerToolBar({
        items: [
            { text: '下载源文件', icon: 'down', click: OutExcel }
        ]
    });

    gridManager = $("#grid").ligerGrid({
        columns: [
            { display: '客户名称', name: 'CDD_Name', minWidth: 80 },
            { display: '计费号码', name: 'CDD_Phone', minWidth: 80 },
            { display: '计费生效时间', name: 'CDD_ActiveDate', minWidth: 80 }

        ], isScoll: false,
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