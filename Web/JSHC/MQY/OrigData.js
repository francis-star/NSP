var Address = "../../"; //当前的html与Aspx中PubForm的文件深度

$(function () {
    InitControl();
    GetArea1();
    InitData();
});

function InitControl() {
    $("#ProDateStart").ligerDateEditor({ format: "yyyy-MM-dd", width: 180 });
    $("#ProDateEnd").ligerDateEditor({ format: "yyyy-MM-dd", width: 180 });

    $("#Province").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value5",
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });

    $("#City").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value6",
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });

    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '查询', id: 'btnSearch', icon: 'search', click: onToFirst },
            { line: true },
            { text: '导入', id: 'btnImport', icon: 'down', click: ShowDialog },
            { line: true },
            { text: '筛选', id: 'btnFilter', icon: 'pager', click: DataScreen },
            { line: true },
            { text: '修改', id: 'btnUpdate', icon: 'edit', click: UpdateDatas },
            { line: true },
            { text: '无效数据', id: 'InvalidData', icon: 'delete', click: ShowInvalidData },
            { line: true },
            { text: '标记为已使用', id: 'MarkData', icon: 'ok', click: MarkAlready },
            { line: true },
            { text: '删除', id: 'btnDelete', icon: 'delete', click: DeleteDatas },
            { line: true },
            { text: '查看', id: 'btnView', icon: 'search2', click: View },
        ]
    });
    /********权限******/
    GetMenuRight(toptoolbarManager, "2016112311930");
    gridManager = $("#grid").ligerGrid({
        columns: [
            { display: '编码', name: 'OD_Code', minWidth: 90 },
            { display: '提供来源', name: 'OD_Provider', minWidth: 90 },
            { display: '提供时间', name: 'OD_ProviderTime', minWidth: 90 },
            { display: '地区', name: 'AreaName', minWidth: 50 },
            {
                display: '数据总条数', name: 'OD_TotalCount', minWidth: 50,
                render: function (record, rowindex, value, column) {
                    return "<a href='javascript:OrigDataDts(\"" + record.OD_Code + "\"," + record.OD_TotalCount + ")'>" + value + "</a>";
                }
            },
            {
                display: '有效条数', name: 'OD_ValidCount', minWidth: 50,
                render: function (record, rowindex, value, column) {
                    return "<a href='javascript:VaildData(\"" + record.OD_Code + "\"," + record.OD_TotalCount + ")'>" + value + "</a>";
                }
            },
            {
                display: '无效条数', name: 'OD_NoValidCount', minWidth: 50,
                render: function (record, rowindex, value, column) {
                    return "<a href='javascript:NOVaildData(\"" + record.OD_Code + "\"," + record.OD_TotalCount + ")'>" + value + "</a>";
                }
            },
            {
                display: '营销成功', name: 'OD_AlreadyUse', minWidth: 50
            },
            {
                display: '状态', name: 'OD_State', minWidth: 115
                , render: function (rowdata, rowindex, value) {
                    var h = "";
                    if (rowdata.OD_State == '未筛选') {
                        h += "<div style='color: Red'>" + rowdata.OD_State + "</div>";

                    }
                    if (rowdata.OD_State == '已筛选') {
                        h += "<div style='color: blue'>" + rowdata.OD_State + "</div>";

                    }
                    if (rowdata.OD_State == '已使用') {
                        h += "<div style='color: green'>" + rowdata.OD_State + "</div>";

                    }
                    return h;
                }
            }
        ],
        pageSize: 20, width: '98%', height: '98%',
        pageSizeOptions: 20,
        enabledSort: false,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast
    });

    //GridInitControl();
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
        width: 135,
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
                    width: 135,
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
        width: 135,
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
    //$( $(".l-toolbar div span")[0]).css("color", "red");

    var arr = new Array();
    arr[0] = $("#Provider").val();
    arr[1] = $("#ProDateStart").val();
    arr[2] = $("#ProDateEnd").val();
    arr[3] = $("#Province").ligerComboBox().getValue() == "D0000001" ? "" : $("#Province").ligerComboBox().getValue();
    arr[4] = $("#City").ligerComboBox().getValue();
    arr[5] = PageIndex;
    arr[6] = PageNum;

    GridLoadAll("BLL_OriginalData", "GetMQYOriginalData", "GetMQYOriginalDataCount", arr);
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

//查看
function View() {
    var rows = gridManager.getCheckedRows();
    if (rows == null || rows == "") {
        $.ligerDialog.warn('请选择行！');
        return false;
    }

    top.layer.open({
        type: 2,
        title: '明细数据',
        shadeClose: false,
        shade: 0.3,
        area: ['65%', '70%'],
        content: '../Form/MQY/frm_View.htm?code=' + rows[0].OD_Code
    });
}

//明细数据
function OrigDataDts(OD_Code, OD_TotleCount) {
    if (OD_TotleCount == 0) {
        $.ligerDialog.warn('未筛选，没有数据');
        return;
    } else {
        top.layer.open({
            type: 2,
            title: '明细数据',
            shadeClose: false,
            shade: 0.3,
            area: ['50%', '50%'],
            content: '../Form/MQY/frm_OrigDataDts.htm?code=' + OD_Code
        });
    }
}

//有效条数
function VaildData(OD_Code, OD_TotleCount) {
    if (OD_TotleCount == 0) {
        $.ligerDialog.warn('未筛选，没有数据');
        return;
    }

    top.layer.open({
        type: 2,
        title: '查看有效数据',
        shadeClose: false,
        shade: 0.3,
        area: ['50%', '50%'],
        content: '../Form/MQY/frm_ValidData.htm?code=' + OD_Code
    });
}

//无效条数
function NOVaildData(OD_Code, OD_TotleCount) {
    if (OD_TotleCount == 0) {
        $.ligerDialog.warn('未筛选，没有数据');
        return;
    }

    top.layer.open({
        type: 2,
        title: '查看无效数据',
        shadeClose: false,
        shade: 0.3,
        area: ['50%', '50%'],
        content: '../Form/MQY/frm_NOVaildData.htm?code=' + OD_Code
    });
}

//导入
function ShowDialog() {
    top.layer.open({
        type: 2,
        title: '导入',
        shadeClose: false,
        shade: 0.3,
        area: ['35%', '68%'],
        content: '../Form/MQY/frm_ImportData.htm',
        end: function () {
            InitData();
        }
    });
}

//无效数据【删除】
function ShowInvalidData() {
    top.layer.open({
        type: 2,
        title: '删除无效数据',
        shadeClose: false,
        shade: 0.3,
        area: ['35%', '50%'],
        content: '../Form/MQY/frm_DInvalidData.htm',
        end: function () {
            InitData();
        }
    });
}

//筛选
function DataScreen() {
    var rows = gridManager.getCheckedRows();

    if (rows == null || rows == "") {
        $.ligerDialog.warn('请选择筛选的行！');
        return false;
    }

    if (rows.length > 1) {
        $.ligerDialog.warn('只能选择一行！');
        return false;
    }

    if (rows[0].OD_State == "已使用") {
        $.ligerDialog.warn('该数据已使用，无法进行筛选！');
        return false;
    }

    var str = rows[0].OD_Code;
    top.layer.open({
        type: 2,
        title: '筛选',
        shadeClose: false,
        shade: 0.3,
        area: ['65%', '70%'],
        content: '../Form/MQY/frm_DataScreen.htm?code=' + str,
        end: function () {
            InitData();
        }
    });
}

//标记为已使用
function MarkAlready() {
    var rows = gridManager.getCheckedRows();
    if (rows == null || rows == "") {
        $.ligerDialog.warn('请选择标记行！');
        return false;
    }

    if (rows.length > 1) {
        $.ligerDialog.warn('只能选择一行！');
        return false;
    }

    if (rows[0].OD_State != "已筛选") {
        $.ligerDialog.warn('该功能只针对于“已筛选”状态下的数据！');
        return false;
    }

    $.ligerDialog.confirm('确认将此数据状态标记为“已使用”？', function (yes) {
        if (yes) {
            var arr = new Array();
            arr[0] = rows[0].OD_Code;
            ReturnValue("BLL_OriginalData", "MarkMQYAlreadyUse", arr, Address, "");
            InitData();
        }
    });
}

//删除
function DeleteDatas() {
    var rows = gridManager.getCheckedRows();
    if (rows == null || rows == "") {
        $.ligerDialog.warn('请选择删除行！');
        return false;
    }

    if (rows.length > 1) {
        $.ligerDialog.warn('只能选择一行！');
        return false;
    }

    if (rows[0].OD_State == "已使用") {
        $.ligerDialog.warn('该数据已被使用，无法删除！');
        return false;
    }

    $.ligerDialog.confirm('确定删除该数据？', function (yes) {
        if (yes) {
            var arr = new Array();
            arr[0] = rows[0].OD_Code;
            ReturnMethod("BLL_OriginalData", "DeleteMQYDatas", arr, Address, "DeleteSucess");
        }
    });
}

//修改
function UpdateDatas() {
    var rows = gridManager.getCheckedRows();
    if (rows == null || rows == "") {
        $.ligerDialog.warn('请选择修改行！');
        return false;
    }
    if (rows.length > 1) {
        $.ligerDialog.warn('只能选择一行！');
        return false;
    }

    if (rows[0].OD_State == "已使用") {
        $.ligerDialog.warn('该数据当前状态为“已使用”，无法进行修改！');
        return false;
    }

    top.layer.open({
        type: 2,
        title: '修改',
        shadeClose: false,
        shade: 0.3,
        area: ['30%', '45%'],
        content: '../Form/MQY/frm_UpdateOrigData.htm?Code=' + rows[0].OD_Code,
        end: function () {
            InitData();
        }
    });
}

var CustomMethod = {
    DeleteSucess: function (text) {
        if (text == 'false') {
            $.ligerDialog.warn('删除失败！');
        }
        else {
            $.ligerDialog.success('删除成功！');
            Search();
        }
    }
}