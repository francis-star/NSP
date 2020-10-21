/**
 * Created by zhiguo on 2016/6/28.
 * ����������js
 */
(function (w, d, $) {
    var Tool = {};

    //���tab�л��¼�
    Tool.tab = function (main, content, cur, cback) {
        var main = $(main), content = $(content);
        main.unbind("click").on("click", function () {
            var index = $(this).index();
            $(this).addClass(cur).siblings().removeClass(cur);
            content.hide().eq(index).show();
            typeof  cback === "function" && cback(index);
        });
    }

    //���޴����ۼ�
    Tool.addnum = function (v) {
        return v += 1;
    };

    //�رյ���
    Tool.closeLay = function (dom, content,mask) {
        var main = $(dom), content = $(content),mask = $(mask);
        main.click(function () {
            content.hide();
            mask.hide();
            $("body,html").attr("style","overflow:visible");
        });
    }

    //��������
    Tool.openLay = function (dom, content,mask) {
        var main = $(dom), content = $(content),mask = $(mask);
        main.click(function () {
            content.show();
            mask.show();
            $("body,html").attr("style","overflow:hidden");
        });
    }
    w.Tool = Tool;
})(window, document, Zepto)