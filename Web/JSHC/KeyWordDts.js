var Address = "../";
var Data = "";
var Data1 = "";
var comboArea, comboType;
$(function () {
    InitControl();
});

function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [{ text: '新增', id: "search", icon: 'search', click: onToFirst },
        { line: true },
        {
            text: '取消', icon: 'logout', click: function (item) {
                parent.layer.closeAll('iframe');
            }
        }]
    });

    comboArea = $("#Province").ligerComboBox({
        width: 300,
        selectBoxWidth: 200, valueField: 'id', textField: "text",
        tree: { url: '../jshc/areaJson.txt', ajaxType: 'get', idFieldName: 'id', textFieldName: "text" }
        //tree: { treeData }
    });

    comboArea.selectValue("D0000100");

    comboType = $("#PlatForm").ligerComboBox({
        width: 300,
        selectBoxWidth: 200, valueField: 'id', textField: "text",
        tree: { url: '../jshc/busyType.txt', ajaxType: 'get', idFieldName: 'id', textFieldName: "text" }
    });
}

function onToFirst() {
    if ($("#KM_Name").val() == "") {
        layer.msg('关键字不能为空！');
        return;
    }
    if (comboType.getValue() == "") {
        layer.msg("业务名称不能为空!");
        return;
    }
    if (comboArea.getValue() == "") {
        layer.msg('地区不能为空！');
        return;
    }

    var arr = new Array();
    arr[0] = comboArea.getValue();
    arr[1] = $("#KM_Name").val(); 
    arr[2] = comboType.getValue();
    arr[3] = $("#Danger").val();
    var retrunValue = ReturnValue("BLL_KeyWordDts", "SaveWord", arr, Address);

    if (retrunValue == "True") {
        $.ligerDialog.success('保存成功！', '提示', function () {
            parent.layer.closeAll('iframe');
        });
    }
    else {
        $.ligerDialog.error('新建失败！');
    }
}

var CustomMethod = {
    SaveDataSuccess: function (text) { 
        if (text == "True") { 
            $.ligerDialog.success('保存成功！', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        }
        else {
            $.ligerDialog.error('新建失败！');
        }
    }
}