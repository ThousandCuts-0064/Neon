export const escapeHtml = function (unsafe: string) {
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