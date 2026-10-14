window.leafsteroidsJson = {
    show: function (elementId) {
        var host = document.getElementById(elementId);
        var source = document.getElementById(elementId + "-source");
        if (!host || !source) return;

        var text = source.textContent || "";
        if (!text.trim()) return;

        if (!window.JSONFormatter) {
            host.textContent = text;
            return;
        }

        // Recordings are MongoDB shell JSON (ObjectId(), ISODate()), which JSON.parse rejects.
        var strict = text
            .replace(/ObjectId\(\s*"([0-9a-fA-F]+)"\s*\)/g, '{"$oid":"$1"}')
            .replace(/ISODate\(\s*"([^"]+)"\s*\)/g, '{"$date":"$1"}')
            .replace(/NumberLong\(\s*"?(-?\d+)"?\s*\)/g, "$1")
            .replace(/NumberInt\(\s*"?(-?\d+)"?\s*\)/g, "$1");

        var data;
        try {
            data = JSON.parse(strict);
            if (typeof data === "string") data = JSON.parse(data);
        } catch (err) {
            host.textContent = text;
            return;
        }

        host.replaceChildren();
        var formatter = new JSONFormatter(data, 1, {
            hoverPreviewEnabled: true,
            hoverPreviewArrayCount: 12,
            hoverPreviewFieldCount: 6,
            theme: "dark",
            animateOpen: true,
            animateClose: true
        });
        host.appendChild(formatter.render());
    }
};
