!function(e,t){"object"==typeof exports&&"object"==typeof module?module.exports=t(require("dnn-svg-icons"),require("react"),require("react-collapse"),require("react-custom-scrollbars"),require("react-dom")):"function"==typeof define&&define.amd?define(["dnn-svg-icons","react","react-collapse","react-custom-scrollbars","react-dom"],t):"object"==typeof exports?exports.Dropdown=t(require("dnn-svg-icons"),require("react"),require("react-collapse"),require("react-custom-scrollbars"),require("react-dom")):e.Dropdown=t(e["dnn-svg-icons"],e.react,e["react-collapse"],e["react-custom-scrollbars"],e["react-dom"])}(this,function(e,t,n,o,r){return function(e){function t(o){if(n[o])return n[o].exports;var r=n[o]={exports:{},id:o,loaded:!1};return e[o].call(r.exports,r,r.exports,t),r.loaded=!0,r.exports}var n={};return t.m=e,t.c=n,t.p="",t(0)}([function(e,t,n){"use strict";function o(e){return e&&e.__esModule?e:{"default":e}}function r(e,t){if(!(e instanceof t))throw new TypeError("Cannot call a class as a function")}function i(e,t){if(!e)throw new ReferenceError("this hasn't been initialised - super() hasn't been called");return!t||"object"!=typeof t&&"function"!=typeof t?e:t}function l(e,t){if("function"!=typeof t&&null!==t)throw new TypeError("Super expression must either be null or a function, not "+typeof t);e.prototype=Object.create(t&&t.prototype,{constructor:{value:e,enumerable:!1,writable:!0,configurable:!0}}),t&&(Object.setPrototypeOf?Object.setPrototypeOf(e,t):e.__proto__=t)}Object.defineProperty(t,"__esModule",{value:!0});var a=function(){function e(e,t){for(var n=0;n<t.length;n++){var o=t[n];o.enumerable=o.enumerable||!1,o.configurable=!0,"value"in o&&(o.writable=!0),Object.defineProperty(e,o.key,o)}}return function(t,n,o){return n&&e(t.prototype,n),o&&e(t,o),t}}(),s=n(17),p=o(s),c=n(20),d=o(c),u=n(18),f=o(u),h=n(19),b=o(h),v=n(16),m=n(13),w=o(m),g=n(7),y=o(g);n(15);var x=function(e){function t(){r(this,t);var e=i(this,(t.__proto__||Object.getPrototypeOf(t)).call(this));return e.state={dropDownOpen:!1,fixedHeight:0,dropdownText:"",closestValue:null,selectedOption:{}},e.handleClick=e.handleClick.bind(e),e.uniqueId=Date.now()*Math.random(),e.debouncedSearch=(0,y["default"])(e.searchItems,500),e}return l(t,e),a(t,[{key:"toggleDropdown",value:function(){var e=this,t=this.props;t.enabled?(this.state.dropDownOpen?this.setState({closestValue:null}):setTimeout(function(){e.setState({}),d["default"].findDOMNode(e.refs.dropdownSearch).focus()},250),this.setState({dropDownOpen:!this.state.dropDownOpen})):this.setState({dropDownOpen:!1})}},{key:"getDropdownHeight",value:function(e,t){var n=this.props;if(n.fixedHeight)return n.fixedHeight;var o=("large"===t?38:28)*e;return o<140?o+20:160}},{key:"componentWillMount",value:function(){var e=this.props;if(e.options&&e.options.length>0){var t=this.getDropdownHeight(e.options.length,e.size);this.setState({fixedHeight:t})}}},{key:"componentWillReceiveProps",value:function(e){if(e.options&&e.options.length>0){var t=this.getDropdownHeight(e.options.length,e.size);this.setState({fixedHeight:t})}}},{key:"componentDidMount",value:function(){var e=this.props;e.closeOnClick&&document.addEventListener("mousedown",this.handleClick),this._isMounted=!0}},{key:"componentWillUnmount",value:function(){document.removeEventListener("mousedown",this.handleClick),this._isMounted=!1}},{key:"handleClick",value:function(e){var t=this.props;this._isMounted&&t.closeOnClick&&(d["default"].findDOMNode(this).contains(e.target)||this.setState({dropDownOpen:!1,closestValue:null,dropdownText:""}))}},{key:"onSelect",value:function(e){var t=this.props;t.enabled&&(this.setState({dropDownOpen:!1,closestValue:null,dropdownText:""}),t.onSelect&&(this.setState({selectedOption:e}),t.onSelect(e)))}},{key:"getClassName",value:function(){var e=this.props,t=this.state,n="dnn-dropdown";return n+=e.withBorder?" with-border":"",n+=" "+e.size,n+=" "+e.className,n+=e.enabled?t.dropDownOpen?" open":"":" disabled"}},{key:"getDropdownLabel",value:function(){var e=this.props,t=e.label;if(void 0!==e.value&&void 0!==e.options&&e.options.length>0){var n=e.options.find(function(t){return t.value===e.value});n&&n.label&&(t=n.label)}return e.prependWith?p["default"].createElement("span",{className:"dropdown-prepend"},p["default"].createElement("strong",null,e.prependWith)," ",t):t}},{key:"getIsMultiLineLabel",value:function(){return this.props.labelIsMultiLine?"":" no-wrap"}},{key:"searchItems",value:function(){var e=this,t=0,n=null,o=0;if(this.props.options.forEach(function(r,i){var l=e.state.dropdownText.replace(/([.?*+^$[\]\\(){}|-])/g,"\\$1"),a=new RegExp("^"+l,"gi"),s="string"==typeof r.label?r.label:r.searchableValue||"";s.match(a)&&s.match(a).length>t&&(t=s.match(a).length,n=r,o=i)}),null===n&&this.props.options.forEach(function(r,i){var l=e.state.dropdownText.replace(/([.?*+^$[\]\\(){}|-])/g,"\\$1"),a=new RegExp(l,"gi"),s="string"==typeof r.label?r.label:r.searchableValue||"";s.match(a)&&s.match(a).length>t&&(t=s.match(a).length,n=r,o=i)}),this.setState({closestValue:n},function(){setTimeout(function(){e.setState({dropdownText:""})},1500)}),null!==n){var r=d["default"].findDOMNode(this.refs[this.uniqueId+"option-"+o]);if(r){var i=r.offsetTop-165;w["default"].top(d["default"].findDOMNode(this.refs.dropdownScrollContainer).childNodes[0],i)}}}},{key:"onDropdownSearch",value:function(e){var t=this;this.setState({dropdownText:e.target.value},function(){t.debouncedSearch()})}},{key:"onKeyDown",value:function(e){"Enter"===e.key&&(this.state.closestValue&&null!==this.state.closestValue.value?(this.onSelect(this.state.closestValue),d["default"].findDOMNode(this.refs.dropdownSearch).blur()):this.onSelect(this.state.selectedOption))}},{key:"render",value:function(){var e=this,t=this.props,n=this.state,o=t.options&&t.options.map(function(o,r){return p["default"].createElement("li",{onClick:e.onSelect.bind(e,o),key:e.uniqueId+"option-"+r,ref:e.uniqueId+"option-"+r,className:o.value===t.value&&null===n.closestValue||o.value===(n.closestValue&&n.closestValue.value)?"selected":""},o.label)});return p["default"].createElement("div",{className:this.getClassName(),style:t.style},p["default"].createElement("div",{className:"collapsible-label"+this.getIsMultiLineLabel(),onClick:this.toggleDropdown.bind(this)},this.getDropdownLabel()),p["default"].createElement("input",{type:"text",onChange:this.onDropdownSearch.bind(this),ref:"dropdownSearch",value:this.state.dropdownText,onKeyDown:this.onKeyDown.bind(this),style:{position:"absolute",opacity:0,pointerEvents:"none",width:0,height:0,padding:0,margin:0}}),t.withIcon&&p["default"].createElement("div",{className:"dropdown-icon",dangerouslySetInnerHTML:{__html:v.ArrowDownIcon},onClick:this.toggleDropdown.bind(this)}),p["default"].createElement("div",{className:"collapsible-content"+(n.dropDownOpen?" open":"")},p["default"].createElement(f["default"],{fixedHeight:n.fixedHeight,keepCollapsedContent:!0,isOpened:n.dropDownOpen},p["default"].createElement(b["default"],{autoHide:!0,style:t.scrollAreaStyle,ref:"dropdownScrollContainer"},p["default"].createElement("div",null,p["default"].createElement("ul",null,o))),!t.fixedHeight&&p["default"].createElement("ul",null,o))))}}]),t}(s.Component);x.propTypes={label:s.PropTypes.string,fixedHeight:s.PropTypes.number,collapsibleWidth:s.PropTypes.number,collapsibleHeight:s.PropTypes.number,keepCollapsedContent:s.PropTypes.bool,className:s.PropTypes.string,scrollAreaStyle:s.PropTypes.object,options:s.PropTypes.array,onSelect:s.PropTypes.func,size:s.PropTypes.string,withBorder:s.PropTypes.bool,withIcon:s.PropTypes.bool,enabled:s.PropTypes.bool,value:s.PropTypes.oneOfType([s.PropTypes.number,s.PropTypes.string]),closeOnClick:s.PropTypes.bool,prependWith:s.PropTypes.string,labelIsMultiLine:s.PropTypes.bool},x.defaultProps={label:"-- Select --",withIcon:!0,withBorder:!0,size:"small",closeOnClick:!0,enabled:!0,className:""},t["default"]=x},function(e,t){function n(e){var t=typeof e;return!!e&&("object"==t||"function"==t)}e.exports=n},function(e,t,n){t=e.exports=n(3)(),t.push([e.id,"svg{fill:#c8c8c8}svg:hover{fill:#6f7273}svg:active{fill:#1e88c3}.dnn-dropdown{position:relative;display:inline-block;cursor:pointer}.dnn-dropdown.disabled{background:#e5e7e6;color:#959695;cursor:not-allowed}.dnn-dropdown.disabled svg{fill:#959695}.dnn-dropdown.disabled .collapsible-label{cursor:not-allowed}.dnn-dropdown.open .collapsible-content{background:#fff;border:1px solid #c8c8c8;margin-top:3px}.dnn-dropdown.open.with-border{border:1px solid #1e88c3}.dnn-dropdown.with-border{border:1px solid #959695}.dnn-dropdown.with-border .collapsible-content{box-shadow:none;margin-top:0}.dnn-dropdown.with-border .collapsible-content.open{border:1px solid #c8c8c8;border-top:none}.dnn-dropdown.with-border.disabled{border:1px solid #e5e7e6}.dnn-dropdown.large{padding:12px 40px 12px 15px}.dnn-dropdown.large .dropdown-icon{top:15px}.dnn-dropdown.large .collapsible-label{font-size:14px}.dnn-dropdown.large .collapsible-content{top:43px}.dnn-dropdown.large .collapsible-content ul li{padding:11px 15px}.dnn-dropdown .dropdown-icon{position:absolute;width:13px;height:13px;right:10px;top:9px}.dnn-dropdown .dropdown-icon svg{fill:#6f7273}.dnn-dropdown .collapsible-label{cursor:pointer;font-family:inherit;position:relative;width:100%;font-size:13px;border:none;color:#46292b;overflow:hidden;text-overflow:ellipsis;padding:8px 40px 8px 15px}.dnn-dropdown .collapsible-label.no-wrap{white-space:nowrap}.dnn-dropdown .collapsible-label .dropdown-prepend>strong{margin-right:5px}.dnn-dropdown .collapsible-content{position:absolute;padding:0;left:-1px;top:102%;width:100%;box-sizing:content-box;background-color:#fff;z-index:1000;box-shadow:0 0 20px 0 rgba(0,0,0,.2)}.dnn-dropdown .collapsible-content ul{padding:10px 0 0}.dnn-dropdown .collapsible-content ul li{padding:6px 15px;text-indent:0;cursor:pointer;font-size:13px;color:#6f7273}.dnn-dropdown .collapsible-content ul li.selected{color:#1e88c3}.dnn-dropdown .collapsible-content ul li:hover{background-color:#eff0f0;color:#1e88c3}",""])},function(e,t){e.exports=function(){var e=[];return e.toString=function(){for(var e=[],t=0;t<this.length;t++){var n=this[t];n[2]?e.push("@media "+n[2]+"{"+n[1]+"}"):e.push(n[1])}return e.join("")},e.i=function(t,n){"string"==typeof t&&(t=[[null,t,""]]);for(var o={},r=0;r<this.length;r++){var i=this[r][0];"number"==typeof i&&(o[i]=!0)}for(r=0;r<t.length;r++){var l=t[r];"number"==typeof l[0]&&o[l[0]]||(n&&!l[2]?l[2]=n:n&&(l[2]="("+l[2]+") and ("+n+")"),e.push(l))}},e}},function(e,t){(function(t){"undefined"!=typeof window?e.exports=window:"undefined"!=typeof t?e.exports=t:"undefined"!=typeof self?e.exports=self:e.exports={}}).call(t,function(){return this}())},function(e,t){(function(t){var n="object"==typeof t&&t&&t.Object===Object&&t;e.exports=n}).call(t,function(){return this}())},function(e,t,n){var o=n(5),r="object"==typeof self&&self&&self.Object===Object&&self,i=o||r||Function("return this")();e.exports=i},function(e,t,n){function o(e,t,n){function o(t){var n=w,o=g;return w=g=void 0,S=t,x=e.apply(o,n)}function c(e){return S=e,T=setTimeout(f,t),k?o(e):x}function d(e){var n=e-O,o=e-S,r=t-n;return D?p(r,y-o):r}function u(e){var n=e-O,o=e-S;return void 0===O||n>=t||n<0||D&&o>=y}function f(){var e=i();return u(e)?h(e):void(T=setTimeout(f,d(e)))}function h(e){return T=void 0,C&&w?o(e):(w=g=void 0,x)}function b(){void 0!==T&&clearTimeout(T),S=0,w=O=g=T=void 0}function v(){return void 0===T?x:h(i())}function m(){var e=i(),n=u(e);if(w=arguments,g=this,O=e,n){if(void 0===T)return c(O);if(D)return T=setTimeout(f,t),o(O)}return void 0===T&&(T=setTimeout(f,t)),x}var w,g,y,x,T,O,S=0,k=!1,D=!1,C=!0;if("function"!=typeof e)throw new TypeError(a);return t=l(t)||0,r(n)&&(k=!!n.leading,D="maxWait"in n,y=D?s(l(n.maxWait)||0,t):y,C="trailing"in n?!!n.trailing:C),m.cancel=b,m.flush=v,m}var r=n(1),i=n(10),l=n(11),a="Expected a function",s=Math.max,p=Math.min;e.exports=o},function(e,t){function n(e){return!!e&&"object"==typeof e}e.exports=n},function(e,t,n){function o(e){return"symbol"==typeof e||r(e)&&a.call(e)==i}var r=n(8),i="[object Symbol]",l=Object.prototype,a=l.toString;e.exports=o},function(e,t,n){var o=n(6),r=function(){return o.Date.now()};e.exports=r},function(e,t,n){function o(e){if("number"==typeof e)return e;if(i(e))return l;if(r(e)){var t="function"==typeof e.valueOf?e.valueOf():e;e=r(t)?t+"":t}if("string"!=typeof e)return 0===e?e:+e;e=e.replace(a,"");var n=p.test(e);return n||c.test(e)?d(e.slice(2),n?2:8):s.test(e)?l:+e}var r=n(1),i=n(9),l=NaN,a=/^\s+|\s+$/g,s=/^[-+]0x[0-9a-f]+$/i,p=/^0b[01]+$/i,c=/^0o[0-7]+$/i,d=parseInt;e.exports=o},function(e,t,n){function o(e){var t=+new Date,n=Math.max(0,16-(t-l)),o=setTimeout(e,n);return l=t,o}var r=n(4),i=r.requestAnimationFrame||r.webkitRequestAnimationFrame||r.mozRequestAnimationFrame||o,l=+new Date,a=r.cancelAnimationFrame||r.webkitCancelAnimationFrame||r.mozCancelAnimationFrame||clearTimeout;Function.prototype.bind&&(i=i.bind(r),a=a.bind(r)),t=e.exports=i,t.cancel=a},function(e,t,n){function o(e,t,n,o,l){function a(){d=!0}function s(o){if(d)return l(new Error("Scroll cancelled"),t[e]);var r=+new Date,a=Math.min(1,(r-p)/f),h=u(a);t[e]=h*(n-c)+c,a<1?i(s):l(null,t[e])}var p=+new Date,c=t[e],d=!1,u=r,f=350;return"function"==typeof o?l=o:(o=o||{},u=o.ease||u,f=o.duration||f,l=l||function(){}),c===n?l(new Error("Element already at target scroll position"),t[e]):(i(s),a)}function r(e){return.5*(1-Math.cos(Math.PI*e))}var i=n(12);e.exports={top:function(e,t,n,r){return o("scrollTop",e,t,n,r)},left:function(e,t,n,r){return o("scrollLeft",e,t,n,r)}}},function(e,t,n){function o(e,t){for(var n=0;n<e.length;n++){var o=e[n],r=f[o.id];if(r){r.refs++;for(var i=0;i<r.parts.length;i++)r.parts[i](o.parts[i]);for(;i<o.parts.length;i++)r.parts.push(p(o.parts[i],t))}else{for(var l=[],i=0;i<o.parts.length;i++)l.push(p(o.parts[i],t));f[o.id]={id:o.id,refs:1,parts:l}}}}function r(e){for(var t=[],n={},o=0;o<e.length;o++){var r=e[o],i=r[0],l=r[1],a=r[2],s=r[3],p={css:l,media:a,sourceMap:s};n[i]?n[i].parts.push(p):t.push(n[i]={id:i,parts:[p]})}return t}function i(e,t){var n=v(),o=g[g.length-1];if("top"===e.insertAt)o?o.nextSibling?n.insertBefore(t,o.nextSibling):n.appendChild(t):n.insertBefore(t,n.firstChild),g.push(t);else{if("bottom"!==e.insertAt)throw new Error("Invalid value for parameter 'insertAt'. Must be 'top' or 'bottom'.");n.appendChild(t)}}function l(e){e.parentNode.removeChild(e);var t=g.indexOf(e);t>=0&&g.splice(t,1)}function a(e){var t=document.createElement("style");return t.type="text/css",i(e,t),t}function s(e){var t=document.createElement("link");return t.rel="stylesheet",i(e,t),t}function p(e,t){var n,o,r;if(t.singleton){var i=w++;n=m||(m=a(t)),o=c.bind(null,n,i,!1),r=c.bind(null,n,i,!0)}else e.sourceMap&&"function"==typeof URL&&"function"==typeof URL.createObjectURL&&"function"==typeof URL.revokeObjectURL&&"function"==typeof Blob&&"function"==typeof btoa?(n=s(t),o=u.bind(null,n),r=function(){l(n),n.href&&URL.revokeObjectURL(n.href)}):(n=a(t),o=d.bind(null,n),r=function(){l(n)});return o(e),function(t){if(t){if(t.css===e.css&&t.media===e.media&&t.sourceMap===e.sourceMap)return;o(e=t)}else r()}}function c(e,t,n,o){var r=n?"":o.css;if(e.styleSheet)e.styleSheet.cssText=y(t,r);else{var i=document.createTextNode(r),l=e.childNodes;l[t]&&e.removeChild(l[t]),l.length?e.insertBefore(i,l[t]):e.appendChild(i)}}function d(e,t){var n=t.css,o=t.media;if(o&&e.setAttribute("media",o),e.styleSheet)e.styleSheet.cssText=n;else{for(;e.firstChild;)e.removeChild(e.firstChild);e.appendChild(document.createTextNode(n))}}function u(e,t){var n=t.css,o=t.sourceMap;o&&(n+="\n/*# sourceMappingURL=data:application/json;base64,"+btoa(unescape(encodeURIComponent(JSON.stringify(o))))+" */");var r=new Blob([n],{type:"text/css"}),i=e.href;e.href=URL.createObjectURL(r),i&&URL.revokeObjectURL(i)}var f={},h=function(e){var t;return function(){return"undefined"==typeof t&&(t=e.apply(this,arguments)),t}},b=h(function(){return/msie [6-9]\b/.test(window.navigator.userAgent.toLowerCase())}),v=h(function(){return document.head||document.getElementsByTagName("head")[0]}),m=null,w=0,g=[];e.exports=function(e,t){t=t||{},"undefined"==typeof t.singleton&&(t.singleton=b()),"undefined"==typeof t.insertAt&&(t.insertAt="bottom");var n=r(e);return o(n,t),function(e){for(var i=[],l=0;l<n.length;l++){var a=n[l],s=f[a.id];s.refs--,i.push(s)}if(e){var p=r(e);o(p,t)}for(var l=0;l<i.length;l++){var s=i[l];if(0===s.refs){for(var c=0;c<s.parts.length;c++)s.parts[c]();delete f[s.id]}}}};var y=function(){var e=[];return function(t,n){return e[t]=n,e.filter(Boolean).join("\n")}}()},function(e,t,n){var o=n(2);"string"==typeof o&&(o=[[e.id,o,""]]);n(14)(o,{});o.locals&&(e.exports=o.locals)},function(t,n){t.exports=e},function(e,n){e.exports=t},function(e,t){e.exports=n},function(e,t){e.exports=o},function(e,t){e.exports=r}])});