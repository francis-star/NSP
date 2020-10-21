var Address = "../";
var UserCode = GetQueryString("UserCode");
var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;

$(function () {
    InitControl();
    if (UserCode != null)
        InitData();

});

//初始化控件
function InitControl() {
    $("#tbEntryDate").ligerDateEditor({ format: "yyyy-MM-dd", width: 180 });
    $("#toptoolbar").ligerToolBar({
        items: [
            { text: '保存', icon: 'save', click: Save }
        ]
    });
    var strData = [{ 'text': '男' }, { 'text': '女' }];
    $("#cmbSex").ligerComboBox({
        data: strData,
        //isMultiSelect: true,
        textField: 'text', width: 180
    });
    liger.get("cmbSex").setText("男");
    //var strPlace = [{ 'text': '全部' }, { 'text': '本部' }, { 'text': '苏州' }, { 'text': '徐庄' }, { 'text': '山东' }];
    //$("#tbPlace").ligerComboBox({
    //    data: strPlace,
    //    isMultiSelect: false,
    //    cancelable: false,
    //    textField: 'text', width: 180
    //});
    //liger.get("tbPlace").setText("全部");
    var treeData = eval(ReturnValue("BLL_UserSetDts", "GetMenu", "", Address));
    leftTree = $("#tree").ligerTree({
        nodeWidth: 100,
        data: treeData,
        idFieldName: 'SM_Code',
        textFieldName: 'SM_Name',
        parentIDFieldName: 'SM_PCode',
        checkbox: true,
        slide: true,
        isExpand: 3
    });
}
//初始化数据
function InitData() {
    var arr = new Array();
    arr[0] = UserCode;
    ReturnMethod("BLL_UserSetDts", "GetUser", arr, Address, "GetDataSuccess");
}
//初始化人物权限
function roleSet() {
    var arr = new Array();
    arr[0] = UserCode;
    var Mdata = eval(ReturnValue("BLL_UserSetDts", "GetUserModule", arr, Address));
    var str = ',';
    for (var d in Mdata) {
        str += Mdata[d].UP_SM_Code + ',';
    }
    var parm = function (Data) {
        if (str.indexOf(',' + Data.SM_Code + ',') != -1) {
            return true;
        }
        else {
            return false;
        }
    }
    leftTree.selectNode(parm);
}
//保存人物权限
function roleSave(Code) {
    var UserCode = Code;
    var notes = leftTree.getChecked();
    var arr = new Array();
    arr[0] = UserCode;
    for (var i = 1; i <= notes.length; i++) {
        if (notes[i - 1].data.SM_Kind == 1) {
            if (notes[i - 1].data.SM_Type != 1) {
                arr[arr.length++] = notes[i - 1].data.SM_Code;
            }
            if (notes[i - 1].data.SM_Type == 1) {
                arr[arr.length++] = notes[i - 1].data.SM_Code;
            }
        }
    }
    ReturnValue("BLL_UserSetDts", "SaveUserModule", arr, Address);
}

//保存用户信息        
function Save() {
    if (validate()) {
        var arr = new Array();
        arr[0] = UserCode;
        arr[1] = $("#tbLoginName").val();
        arr[2] = $("#tbPassword").val();
        arr[3] = $("#tbUserName").val();
        arr[4] = $("#cmbSex").val();
        arr[5] = $("#tbAge").val();
        arr[6] = $("#tbPhone").val();
        arr[7] = $("#tbPost").val();
        arr[8] = $("#tbEntryDate").val();
        arr[9] = $("#tbPlace").val();
        ReturnMethod("BLL_UserSetDts", "SaveUser", arr, Address, "SaveDataSuccess");
    }
}
function Close() {
    parent.layer.closeAll('iframe');
}

var CustomMethod = {
    SaveDataSuccess: function (text) {
        var data = text;
        var Code = data;
        roleSave(Code);
        if (Code != "") {
            $.ligerDialog.success('保存成功!', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        }
        else {
            $.ligerDialog.error("保存失败,姓名或用户名重复！");
        }
    },
    GetDataSuccess: function (text) {
        var data = eval(text);
        $("#tbLoginName").val(data[0].User_LoginName);
        $("#tbPassword").val(data[0].User_Password);
        $("#tbUserName").val(data[0].User_Name);
        $("#cmbSex").val(data[0].User_Sex);
        $("#tbAge").val(data[0].User_Age);
        $("#tbPhone").val(data[0].User_Phone);
        $("#tbPost").val(data[0].User_Post);
        $("#tbEntryDate").val(data[0].time);
        $("#tbPlace").val(data[0].User_Place);
        roleSet();
    }
}
//信息输入判定
function validate() {
    if ($("#tbLoginName").val() == "") {
        $.ligerDialog.warn('用户名 不能为空！');
        return false;
    }
    if ($("#tbLoginName").val().length > 50) {
        $.ligerDialog.warn('用户名 最大长度不能超过50！');
        return false;
    }
    if ($("#tbPassword").val() == "") {
        $.ligerDialog.warn('密码 不能为空！');
        return false;
    }
    if ($("#tbPassword").val().length > 50) {
        $.ligerDialog.warn('密码 最大长度不能超过50！');
        return false;
    }
    if ($("#tbUserName").val() == "") {
        $.ligerDialog.warn('姓名 不能为空！');
        return false;
    }
    if ($("#tbUserName").val().length > 50) {
        $.ligerDialog.warn('真实姓名 最大长度不能超过50！');
        return false;
    }
    var Phone = /^1[3|4|5|8][0-9]\d{8}$/;
    var userPhone = $("#tbPhone").val();
    if (!Phone.test(userPhone)) {
        $.ligerDialog.warn("请正确填写电话号码，例如:13415764179");
        return false;
    }
    if ($("#tbPlace").val() == "") {
        $.ligerDialog.warn('工作地点 不能为空！');
        return false;
    }
    if ($("#cmbPost").val() == "") {
        $.ligerDialog.warn('角色 不能为空！');
        return false;
    }
    var notes = leftTree.getChecked();
    if (notes.length == 0) {
        $.ligerDialog.warn("请选择权限！")
        return false;
    }
    return true;
}
