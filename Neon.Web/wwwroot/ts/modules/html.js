export const formatHtml = (html, data) => {
    return html.replace(/{{(.*?)}}/g, (match, key) => {
        return key in data ? data[key] : match;
    });
};
export const escapeHtml = function (unsafe) {
    return unsafe.replace(/[&<>"']/g, match => {
        switch (match) {
            case '&': return '&amp;';
            case '<': return '&lt;';
            case '>': return '&gt;';
            case '"': return '&quot;';
            default: return '&#039;';
        }
    });
};
//# sourceMappingURL=html.js.map