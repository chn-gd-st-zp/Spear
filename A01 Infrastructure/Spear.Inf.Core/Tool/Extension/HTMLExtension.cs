using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Spear.Inf.Core.Tool.Extension
{
    public static class HTMLExtension
    {
        /// <summary>
        /// 移除HTML标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="unRemoveTags"></param>
        /// <returns></returns>
        public static string RemoveTag(string html, string[] unRemoveTags = null)
        {
            if (unRemoveTags == null)
                unRemoveTags = new string[] { };

            string[] otherTags = new string[] { "!doctype html", "base", "!", "object" };

            //过滤注释
            html = RemovePattern(html, @" <!--([^(-){2}])*-->");

            foreach (var tag in otherTags)
            {
                if (unRemoveTags.Contains(tag.ToString()))
                    continue;

                html = RemovePattern(html, TagRegex.Remove(tag.ToString()));
            }

            foreach (var tag in Enum.GetValues(typeof(HtmlType)))
            {
                if (unRemoveTags.Contains(tag.ToString()))
                    continue;

                html = RemovePattern(html, TagRegex.RemoveAll(tag.ToString()));
            }

            foreach (var tag in Enum.GetValues(typeof(HtmlElement)))
            {
                if (unRemoveTags.Contains(tag.ToString()))
                    continue;

                html = RemovePattern(html, TagRegex.Remove(tag.ToString()));
            }

            return html;
        }

        /// <summary>
        /// 移除标记
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string RemovePattern(string html, string pattern)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            html = html.Trim();
            html = Regex.Replace(html, pattern, "");

            return html;
        }

        /// <summary>
        /// 移除JScript代码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveJScriptTag(string html)
        {
            html = RemovePattern(html, @"(<script){1,}[^<>]*>[^\0]*(<\/script>){1,}");
            return html;
        }

        /// <summary>
        /// 移除Div代码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveDivTag(string html)
        {
            html = RemovePattern(html, @"<[\/]{0,1}(div [^<>]*>)|<[\/]{0,1}(div>)");
            return html;
        }

        /// <summary>
        /// 移除Iframe代码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveIframeTag(string html)
        {
            html = RemovePattern(html, @"(<iframe){1,}[^<>]*>[^\0]*(<\/iframe>){1,}");
            return html;
        }
    }

    public class TagRegex
    {
        public static string Remove(string tag)
        {
            return @"<[\/]{0,1}(" + tag + @" [^<>]*>)|<[\/]{0,1}(" + tag + ">)";
        }

        public static string RemoveAll(string tag)
        {
            //return @"(<" + tag + @"){1,}[^<>]*>[^\0]*(<\/" + tag + ">){1,}";
            return @"<" + tag + @"([^>])*>(\w|\W)*?</" + tag + "([^>])*>";
        }
    }

    internal enum HtmlType
    {
        style,//   定义文档的样式信息。
        iframe,//   定义内联框架。  
        script,//   定义客户端脚本。      
    }

    internal enum HtmlElement
    {
        //!--...--,//   定义注释。      
        a,//   定义锚。      
        abbr,//   定义缩写。      
        acronym,//   定义只取首字母的缩写。      
        address,//   定义文档作者或拥有者的联系信息。      
        applet,//   不赞成使用。定义嵌入的 applet。      
        area,//   定义图像地图内部的区域。      
        b,//   定义粗体文本。      
        //base,//   定义页面中所有链接的默认地址或默认目标。      
        baseont,//   不赞成使用。定义页面中文本的默认字体、颜色或尺寸。      
        bdo,//   定义文本的方向。      
        big,//   定义大号文本。      
        blockquote,//   定义块引用。      
        body,//   定义文档的主体。      
        br,//   定义简单的折行。      
        button,//   定义按钮。      
        caption,//   定义表格标题。      
        center,//   不赞成使用。定义居中文本。      
        cite,//   定义引用(citation)。      
        code,//   定义计算机代码文本。      
        col,//   定义表格中一个或多个列的属性值。      
        colgroup,//   定义表格中供格式化的列组。      
        dd,//   定义定义列表中项目的描述。      
        del,//   定义被删除文本。      
        dir,//   不赞成使用。定义目录列表。      
        div,//   定义文档中的节。      
        dl,//   定义定义列表。      
        dn,//   定义定义项目。      
        //DOCTYPE,//    定义文档类型。    
        dt,//   定义定义列表中的项目。      
        em,//   定义强调文本。    
        form,
        h1,// to h6,//   定义 HTML 标题。  
        h2,
        h3,
        h4,
        h5,
        h6,
        head,//   定义关于文档的信息。      
        hr,//   定义水平线。      
        html,//   定义 HTML 文档。      
        i,//   定义斜体文本。      
        ieldset,//   定义围绕表单中元素的边框。      
        img,//   定义图像。      
        input,//   定义输入控件。      
        ins,//   定义被插入文本。      

        isindex,//   不赞成使用。定义与文档相关的可搜索索引。      
        kbd,//   定义键盘文本。      
        label,//   定义 input 元素的标注。      
        legend,//   定义 ieldset 元素的标题。      
        li,//   定义列表的项目。      
        link,//   定义文档与外部资源的关系。      
        map,//   定义图像映射。      
        menu,//   不赞成使用。定义菜单列表。      
        meta,//   定义关于 HTML 文档的元信息。      
        norames,//   定义针对不支持框架的用户的替代内容。      
        noscript,//   定义针对不支持客户端脚本的用户的替代内容。      
        //object,//   定义嵌入的对象。      
        ol,//   定义有序列表。      
        ont,//   不赞成使用。定义文本的字体、尺寸和颜色      
        oot,//   定义表格中的表注内容（脚注）。      
        optgroup,//   定义选择列表中相关选项的组合。      
        option,//   定义选择列表中的选项。      
        orm,//   定义供用户输入的 HTML 表单。      
        p,//   定义段落。      
        param,//   定义对象的参数。
        pre,//   定义预格式文本      
        q,//   定义短的引用。      
        rame,//   定义框架集的窗口或框架。      
        rameset,//   定义框架集。      
        s,//   不赞成使用。定义加删除线的文本。      
        samp,//   定义计算机代码样本。      

        select,//   定义选择列表（下拉列表）。      
        small,//   定义小号文本。      
        span,//   定义文档中的节。      
        strike,//   不赞成使用。定义加删除线的文本。      
        strong,//   定义语气更为强烈的强调文本。      
        sub,//   定义下标文本。      
        sup,//   定义上标文本。      
        table,//   定义表格      
        tbody,//   定义表格中的主体内容。      
        td,//   定义表格中的单元。      
        textarea,//   定义多行的文本输入控件。      
        th,//   定义表格中的表头单元格。      
        thead,//   定义表格中的表头内容。      
        title,//   定义文档的标题。      
        tr,//   定义表格中的行。      
        tt,//   定义打字机文本。      
        u,//   不赞成使用。定义下划线文本。      
        ul,//   定义无序列表。      
        var,//   定义文本的变量部分。      
        xmp,//   不赞成使用。定义预格式文本。       

    }
}