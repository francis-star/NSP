var gridManager = null; //列表控件
var groupicon = "../JS/lib/ligerUI/skins/icons/communication.gif";
var form;
var Address = "../"; //当前的html与Aspx中PubForm的文件深度

var CSet_Code = unescape(GetQueryString("CSet_Code"));
var CSet_CType_Code = unescape(GetQueryString("CSet_CType_Code"));
var CSet_Name = unescape(GetQueryString("CSet_Name"));

$("#City").val(unescape(GetQueryString("OD_CityName")))


$(function () {

    InitControl(); 

    if (CSet_Code != null && CSet_Code != "null") {
        //先把添加隐藏保存 改为显示
        $("div.l-toolbar-item").eq(0).hide();
        $("div.l-bar-separator").eq(0).hide();

        $("#Type").ligerComboBox().setValue(CSet_CType_Code)
        //查询数据加载关键字值
        $("#Name").val(CSet_Name);
    }
    else {
        $("div.l-toolbar-item").eq(1).hide();
        $("div.l-bar-separator").eq(1).hide();
    }
});

function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [{
            text: '添加', id: 'btnAdd', icon: 'add', click: function (item) {
                if ($("#Type").ligerComboBox().getText() == "") {
                    $.ligerDialog.warn('请选择类型！');
                    return false;
                }
                if ($("#Name").val() == "") {
                    $.ligerDialog.warn('请输入名称！');
                    return false;
                }
                else {
                    var arr = new Array();
                    arr[0] = "";
                    arr[1] = $("#Name").val();
                    arr[2] = $("#Type").ligerComboBox().getValue();
                    ReturnMethod("BLL_CodeSet", "UpdateCodeSet", arr, Address, "SaveDataSuccess");
                }
            }
        }, { line: true },
        {
            text: '修改', icon: 'edit', click: function (item) {
                if ($("#Type").ligerComboBox().getText() == "") {
                    $.ligerDialog.warn('请选择类型！');
                    return false;
                }
                if ($("#Name").val() == "") {
                    $.ligerDialog.warn('请输入名称！');
                    return false;
                }
                else {
                    var arr = new Array();
                    arr[0] = CSet_Code;
                    arr[1] = $("#Name").val();
                    arr[2] = $("#Type").ligerComboBox().getValue();
                    ReturnMethod("BLL_CodeSet", "UpdateCodeSet", arr, Address, "SaveDataSuccess");
                }
            }
        }, { line: true },
       {
           text: '退出', icon: 'logout', click: function (item) {
               parent.layer.closeAll('iframe');
           }
       }]
    });

    var dataType = eval(ReturnValue("BLL_CodeSet", "GetCodeType", "", Address));

    $("#Type").ligerComboBox({
        width: 198,
        selectBoxWidth: 198,
        data: dataType,
        textField: "CType_Name",
        valueField: "CType_Code",
        valueFieldID: 'CType_Code',
        cancelable: false
    });
}

//回调刷新列表页
function End() {
    window.close();
    var pWindow = window.dialogArguments; if (pWindow != null) {
        alert(parent);
        parent.pWindow.InitData();

    } else {
        window.opener.InitData();
    }
}

var CustomMethod = {
    SaveDataSuccess: function (text) {
        if (text == "true") { 
            parent.location.reload();  
        }
        else {
            $.ligerDialog.error('保存失败!');
        }
    }
}