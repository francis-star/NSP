var gridManager;
var groupicon = "../JS/lib/ligerUI/skins/icons/communication.gif";
var Address = "../";

$(function () {
    InitControl();
    InitData();
});

function InitControl() {
    var dataType = eval(ReturnValue("BLL_CodeSet", "GetCodeType", "", Address));

    $("#Type").ligerComboBox({
        width: 180,
        selectBoxWidth: 180,
        data: dataType,
        textField: "CType_Name",
        valueField: "CType_Code",
        valueFieldID: 'CType_Code',
        cancelable: false
    });

    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
                    { text: '查询', icon: 'search', click: Search },
                    { line: true },
                    { text: '新增', id: 'btnAdd', icon: 'add', click: Add },
                    { line: true },
                    { text: '修改', id: 'btnUpdate', icon: 'edit', click: UpdateDatas },
                    { line: true },
                    { text: '删除', id: 'btnDelete', icon: 'delete', click: DeleteDatas }
        ]
    });

    gridManager = $("#grid").ligerGrid({
        columns: [
            { display: '编码', name: 'CType_Code', minWidth: 65 },
            { display: '名称', name: 'CType_Name', minWidth: 65 }
        ],
        pageSize: 20, width: '98%', height: '98%',
        pageSizeOptions: 20,
        enabledSort: false
    });
}

function InitData() {
    var arr = new Array();
    arr[0] = $("#Type").ligerComboBox().getValue();
    var Result = eval("(" + ReturnValue("BLL_CodeType", "GetCodeType", arr, Address) + ")");
    gridManager.loadData(Result)
}

//查询
function Search() {
    var arr = new Array();
    arr[0] = $("#Type").ligerComboBox().getValue();

    var Result = eval("(" + ReturnValue("BLL_CodeType", "GetCodeType", arr, Address) + ")");
    gridManager.loadData(Result)
    gridManager.changePage('first');
}

//添加
function Add() {
    layer.open({
        type: 2,
        title: '添加基础类型管理',
        shadeClose: false,
        shade: 0.3,
        area: ['35%', '45%'],
        content: 'frm_AddCodeType.html' //iframe的url
    });
}

//修改
function UpdateDatas() {
    var rows = gridManager.getCheckedRows();
    if (rows == null || rows == "") {
        $.ligerDialog.warn('请选择修改的行！');
        return false;
    }
    if (rows.length > 1) {
        $.ligerDialog.warn('只能选择一行！');
        return false;
    }
    var url = "frm_AddCodeType.html?CType_Code=" + escape(rows[0].CType_Code) + "&CType_Name=" + escape(rows[0].CType_Name);
    layer.open({
        type: 2,
        title: '添加基础类型管理',
        shadeClose: true,
        shade: 0.8,
        area: ['35%', '45%'],
        content: url  //iframe的url
    });
}

//删除
function DeleteDatas() {
    var rows = gridManager.getCheckedRows();
    if (rows == null || rows == "") {
        $.ligerDialog.warn('请选择删除的行！');
        return false;
    }
    if (rows.length > 1) {
        $.ligerDialog.warn('只能选择一行！');
        return false;
    }

    $.ligerDialog.confirm('确定删除该数据？', function (yes) {
        if (yes) {
            var rows = gridManager.getCheckedRows();

            var arr = new Array();
            arr[0] = rows[0].CType_Code;
            ReturnMethod("BLL_CodeType", "DeleteType", arr, Address, "DeleteSucess");
            InitData();
        }
    });
}


var CustomMethod = {
    DeleteSucess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('删除成功！');
            InitData();
        }
        else {
            $.ligerDialog.error('删除失败！');
        }
    }
}