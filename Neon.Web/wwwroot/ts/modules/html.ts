export const formatHtml = (html: string, data: Record<string, string>) => {
	return html.replace(/{{(.*?)}}/g, (match, key) => {
		return key in data ? data[key] : match;
	});
};

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