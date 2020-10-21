var gridManager = null; //列表控件
var groupicon = "../JS/lib/ligerUI/skins/icons/communication.gif";
var form;
var Address = "../"; //当前的html与Aspx中PubForm的文件深度

$(function () {

    InitControl();
    InitData();

});

function InitControl() {

    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
                    { text: '添加节点', id: 'btnAdd', icon: 'add', click: AddData },
                    { line: true },
                    { text: '修改节点', id: 'btnUpdate', icon: 'pager', click: UpdateData },
                    { line: true },
                    { text: '删除节点', id: 'btnDelete', icon: 'edit', click: DeleteData }
        ]
    });

    GetMenuRight(toptoolbarManager, "2016112311936");

    InitData();

}

//初始化数据
function InitData() {
    var treeData = eval(ReturnValue("BLL_LevelSet", "GetLevelSets", "", Address));
    leftTree = $("#tree").ligerTree({
        nodeWidth: 200,
        data: treeData,
        idFieldName: 'LS_Code',
        textFieldName: 'LS_Name',
        parentIDFieldName: 'LS_PCode',
        checkbox: false,
        slide: true,
        isExpand: 3
    });
}

//新增节点
function AddData() {
    var notes = leftTree.getSelected();
    if (notes == "" || notes == null) {
        $.ligerDialog.warn("请选择一个节点!");
        return false;
    }
    layer.open({
        type: 2,
        title: '分类信息设定',
        shadeClose: false,
        shade: 0.3,
        area: ['45%', '45%'],
        content: "frm_LevelSetDts.htm?Ptype=0&Code=" + notes.data.LS_Code + "&PCode=" + notes.data.LS_PCode //iframe的url
    });
    //window.showModalDialog("frm_LevelSetDts.htm?Ptype=0&Code=" + notes.data.LS_Code + "&PCode=" + notes.data.LS_PCode, "分类信息设定", "width=600,height=480,toolbar=no,location=no,directories=no,status=no,menubars=no,scrollbars=yes,resizable=no");
    //InitData();
}

//修改节点
function UpdateData() {
    var notes = leftTree.getSelected();
    if (notes == "" || notes == null) {
        $.ligerDialog.warn("请选择一个节点!");
        return false;
    }
    if (notes.data.LS_Name != "网页" && notes.data.LS_Name != "微信" && notes.data.LS_Name != "APP") {
        var arr = new Array();
        arr[0] = notes.data.LS_Code;
        var Data = eval(ReturnValue("BLL_LevelSet", "GetLevelSet", arr, Address));
        if (Data.length > 0) {
            layer.open({
                type: 2,
                title: '分类信息设定',
                shadeClose: false,
                shade: 0.3,
                area: ['45%', '45%'],
                content: "frm_LevelSetDts.htm?Ptype=1&Code=" + notes.data.LS_Code + "&PCode=" + notes.data.LS_PCode //iframe的url
            });
            //window.showModalDialog("frm_LevelSetDts.htm?Ptype=1&Code=" + notes.data.LS_Code + "&PCode=" + notes.data.LS_PCode, "分类信息设定", "width=600,height=480,toolbar=no,location=no,directories=no,status=no,menubars=no,scrollbars=yes,resizable=no");
            //InitData();

        }
    }
    else {
        $.ligerDialog.warn("不能修改移动端信息!");
    }


}

//删除节点
function DeleteData() {
    var notes = leftTree.getSelected();
    if (notes == "" || notes == null) {
        $.ligerDialog.warn("请选择一个节点!");
        return false;
    }

    if (notes.data.LS_Name != "网页" && notes.data.LS_Name != "微信" && notes.data.LS_Name != "APP") {
        var arr = new Array();
        arr[0] = notes.data.LS_Code;
        var Data = eval(ReturnValue("BLL_LevelSet", "GetLevelSet", arr, Address));
        if (Data.length > 0) {
            $.ligerDialog.confirm('确定删除？', function (yes) {
                if (yes) {
                    var arr = new Array();
                    arr[0] = notes.data.LS_Code;
                    var Data = ReturnValue("BLL_LevelSet", "DeleteLevelSet", arr, Address);
                    if (Data == "0") {
                        $.ligerDialog.success("删除成功!");
                        InitData();
                    }
                    if (Data == "1") {
                        $.ligerDialog.error("删除失败!");
                    }
                    if (Data == "2") {
                        $.ligerDialog.warn("存在子节点，请先删除子节点!");
                    }
                    if (Data == "3") {
                        $.ligerDialog.warn("不能删除添加有信息的节点!");
                    }
                }
            });

        }
    }
    else {
        $.ligerDialog.warn("不能删除移动端信息!");
    }



}
