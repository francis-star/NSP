var gridManager = null; //列表控件
var groupicon = "../JS/lib/ligerUI/skins/icons/communication.gif";
var form;
var Address = "../"; //当前的html与Aspx中PubForm的文件深度

var CType_Code = unescape(GetQueryString("CType_Code"));
var CType_Name = unescape(GetQueryString("CType_Name"));


$(function () {
    InitControl();

    if (CType_Code != null && CType_Code != "null") {
        //先把添加隐藏保存 改为显示
        $("div.l-toolbar-item").eq(0).hide();
        $("div.l-bar-separator").eq(0).hide();
         
        //查询数据加载关键字值
        $("#TypeName").val(CType_Name);
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
                if ($("#TypeName").val() == "") {
                    $.ligerDialog.warn('请输入类型名称！');
                    return false;
                }
                else {
                    var arr = new Array();
                    arr[0] = "";
                    arr[1] = $("#TypeName").val(); 
                    ReturnMethod("BLL_CodeType", "UpdateCodeType", arr, Address, "SaveDataSuccess");
                }
            }
        }, { line: true },
        {
            text: '修改', icon: 'edit', click: function (item) { 
                if ($("#TypeName").val() == "") {
                    $.ligerDialog.warn('请输入类型名称！');
                    return false;
                }
                else {
                    var arr = new Array();
                    arr[0] = CType_Code;
                    arr[1] = $("#TypeName").val(); 
                    ReturnMethod("BLL_CodeType", "UpdateCodeType", arr, Address, "SaveDataSuccess");
                }
            }
        }, { line: true },
       {
           text: '退出', icon: 'logout', click: function (item) {
               parent.layer.closeAll('iframe');
           }
       }]
    });
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
