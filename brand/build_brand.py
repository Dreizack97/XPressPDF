"""Genera el kit de identidad visual en SVG para XPress PDF (Isotipo: Prisma de Conversión Documental).

Uso: python build_brand.py [carpeta_salida]
"""
import os
import re
import sys

# --- Paleta (tomada de AppUI/Shared/Resources/Tokens/Primitives.axaml) -------------
INK = "#0F172A"        # secondary-900 · Pizarra profunda / tinta
BLUE = "#2563EB"       # primary-600   · Azul corporativo (documento técnico)
NAVY = "#1E3A8A"       # primary-900   · Azul profundo (zócalo estructural y contenedor)
NAVY_DEEP = "#172554"  # primary-950   · Azul abisal
EMERALD = "#10B981"    # accent-500    · Esmeralda (vector de conversión rápida / acción)
WHITE = "#FFFFFF"      # Espacio negativo y pliegue técnico

PALETTES = {
    # a: pliego documental · b: zócalo base/contenedor · l: vector XPress · t: nombre · s: sufijo PDF
    "color": dict(a=BLUE, b=NAVY, l=EMERALD, t=INK, s=BLUE),
    "mono": dict(a=INK, b=INK, l=INK, t=INK, s=INK),
    "negativo": dict(a="#93C5FD", b="#3B82F6", l="#34D399", t=WHITE, s="#93C5FD"),
}

ISOLOGO = {
    "color": dict(a=BLUE, b=NAVY, l=EMERALD, t=WHITE, s="#93C5FD"),
    "mono": dict(a=INK, b=INK, l=INK, t=None, s=None),
    "negativo": dict(a="#93C5FD", b=BLUE, l="#34D399", t=WHITE, s="#DBEAFE"),
}

TILES = {
    "claro": (WHITE, PALETTES["color"]),
    "marca": (BLUE, dict(a=WHITE, b=NAVY_DEEP, l="#6EE7B7")),
    "oscuro": (INK, PALETTES["negativo"]),
}

LABEL = {"color": "a color", "mono": "monocromo", "negativo": "en negativo"}

# --- Geometría del isotipo Prisma de Conversión (retícula de 56 × 56) ------
SYMBOL = 56
GAP = 4.0        # Canal de 2 u por lado (estándar)
GAP_SMALL = 6.0  # Canal reforzado para microescala (16–32 px)

DOC_A = "M12.5 6H31L42 17V38H9V9.5Q9 6 12.5 6Z"
FOLD = "M31 6V17H42Z"
GROOVES = "M15 16H25M15 22H27M15 28H22"
BASE_B = "M9 40H45V46.5Q45 50 41.5 50H12.5Q9 50 9 46.5Z"
VECTOR_L = "M17 30H33V23L48 34L33 45V38H17Z"


def fmt(v):
    s = f"{v:.2f}".rstrip("0").rstrip(".")
    return "0" if s == "-0" else s


# --- Tipografía Fira Sans (600 SemiBold y 500 Medium) a trazos ------------
# Glifos vectoriales exactos en base size=40 (upem=1000, k=0.04, cap=27.68).
_GLYPHS_600 = {
    "X": (
        "M22.92 0L16.44 0L11.28 -10.96L6.2 0L-0.08 0L7.8 -14.72L0.56 -27.68L7 -27.68L11.36 -18.44L15.92 -27.68L22.16 -27.68L14.84 -14.92Z",
        (-0.08, -27.68, 22.92, 0.0),
        22.84,
    ),
    "P": (
        "M22.84 -18.76Q22.84 -14.12 19.88 -11.78Q16.92 -9.44 11.96 -9.44L8.8 -9.44L8.8 0L2.92 0L2.92 -27.68L11.48 -27.68Q16.88 -27.68 19.86 -25.44Q22.84 -23.2 22.84 -18.76ZM16.76 -18.76Q16.76 -23.44 11.4 -23.44L8.8 -23.44L8.8 -13.76L11.48 -13.76Q14.04 -13.76 15.4 -14.92Q16.76 -16.08 16.76 -18.76Z",
        (2.92, -27.68, 22.84, 0.0),
        23.8,
    ),
    "r": (
        "M15.8 -21.48L14.88 -16Q13.68 -16.28 12.96 -16.28Q11.08 -16.28 10.02 -14.92Q8.96 -13.56 8.4 -10.84L8.4 0L2.72 0L2.72 -21.2L7.68 -21.2L8.16 -17.08Q8.92 -19.28 10.38 -20.54Q11.84 -21.8 13.68 -21.8Q14.72 -21.8 15.8 -21.48Z",
        (2.72, -21.8, 15.8, 0.0),
        15.84,
    ),
    "e": (
        "M20.6 -8.88L7.36 -8.88Q7.64 -6 8.92 -4.8Q10.2 -3.6 12.4 -3.6Q13.76 -3.6 14.96 -4.04Q16.16 -4.48 17.56 -5.44L19.92 -2.24Q16.36 0.64 11.88 0.64Q6.88 0.64 4.2 -2.36Q1.52 -5.36 1.52 -10.48Q1.52 -13.72 2.66 -16.3Q3.8 -18.88 5.98 -20.36Q8.16 -21.84 11.16 -21.84Q15.68 -21.84 18.2 -19Q20.72 -16.16 20.72 -11.08Q20.72 -10.68 20.6 -8.88ZM15.08 -12.68Q15.08 -17.96 11.28 -17.96Q9.52 -17.96 8.54 -16.66Q7.56 -15.36 7.36 -12.44L15.08 -12.44Z",
        (1.52, -21.84, 20.72, 0.64),
        22.08,
    ),
    "s": (
        "M17.52 -19.28L15.36 -16.04Q12.68 -17.72 10.08 -17.72Q8.68 -17.72 7.92 -17.2Q7.16 -16.68 7.16 -15.8Q7.16 -15.16 7.5 -14.72Q7.84 -14.28 8.78 -13.88Q9.72 -13.48 11.6 -12.92Q14.96 -12 16.6 -10.46Q18.24 -8.92 18.24 -6.24Q18.24 -4.12 17.02 -2.56Q15.8 -1 13.72 -0.18Q11.64 0.64 9.08 0.64Q6.4 0.64 4.2 -0.18Q2 -1 0.44 -2.44L3.24 -5.56Q5.92 -3.48 8.92 -3.48Q10.52 -3.48 11.46 -4.1Q12.4 -4.72 12.4 -5.8Q12.4 -6.64 12.04 -7.12Q11.68 -7.6 10.72 -8.02Q9.76 -8.44 7.72 -9Q4.52 -9.88 2.98 -11.5Q1.44 -13.12 1.44 -15.52Q1.44 -17.32 2.48 -18.76Q3.52 -20.2 5.42 -21.02Q7.32 -21.84 9.84 -21.84Q14.32 -21.84 17.52 -19.28Z",
        (0.44, -21.84, 18.24, 0.64),
        19.08,
    ),
}

_GLYPHS_500 = {
    "P": (
        "M22.6 -18.84Q22.6 -14.24 19.64 -11.94Q16.68 -9.64 11.76 -9.64L8.52 -9.64L8.52 0L3.24 0L3.24 -27.64L11.4 -27.64Q16.72 -27.64 19.66 -25.44Q22.6 -23.24 22.6 -18.84ZM17.08 -18.84Q17.08 -23.76 11.36 -23.76L8.52 -23.76L8.52 -13.6L11.36 -13.6Q14.2 -13.6 15.64 -14.78Q17.08 -15.96 17.08 -18.84Z",
        (3.24, -27.64, 22.6, 0.0),
        23.64,
    ),
    "D": (
        "M24.2 -13.96Q24.2 -6.28 20.4 -3.14Q16.6 0 10.76 0L3.24 0L3.24 -27.64L10.04 -27.64Q16.48 -27.64 20.34 -24.72Q24.2 -21.8 24.2 -13.96ZM8.52 -23.68L8.52 -3.96L11.12 -3.96Q14.64 -3.96 16.64 -6.14Q18.64 -8.32 18.64 -13.96Q18.64 -17.88 17.62 -20.02Q16.6 -22.16 14.94 -22.92Q13.28 -23.68 10.96 -23.68Z",
        (3.24, -27.64, 24.2, 0.0),
        25.92,
    ),
    "F": (
        "M3.24 -27.64L18.88 -27.64L18.32 -23.72L8.52 -23.72L8.52 -15.44L17.04 -15.44L17.04 -11.52L8.52 -11.52L8.52 0L3.24 0Z",
        (3.24, -27.64, 18.88, 0.0),
        19.56,
    ),
}


def _transform_svg_path(d, scale, tx, ty):
    """Escala un comando SVG (M, L, H, V, Q, Z) desde base 40 y traslada por (tx, ty)."""
    tokens = re.findall(r"[MLHVQZ]|-?\d+(?:\.\d+)?", d)
    out = []
    cmd = ""
    i = 0
    while i < len(tokens):
        tok = tokens[i]
        if tok.isalpha():
            cmd = tok
            out.append(cmd)
            i += 1
            if cmd == "Z":
                continue
        if cmd in ("M", "L"):
            x = float(tokens[i]) * scale + tx
            y = float(tokens[i + 1]) * scale + ty
            out.append(f"{fmt(x)} {fmt(y)}")
            i += 2
        elif cmd == "H":
            x = float(tokens[i]) * scale + tx
            out.append(fmt(x))
            i += 1
        elif cmd == "V":
            y = float(tokens[i]) * scale + ty
            out.append(fmt(y))
            i += 1
        elif cmd == "Q":
            x1 = float(tokens[i]) * scale + tx
            y1 = float(tokens[i + 1]) * scale + ty
            x = float(tokens[i + 2]) * scale + tx
            y = float(tokens[i + 3]) * scale + ty
            out.append(f"{fmt(x1)} {fmt(y1)} {fmt(x)} {fmt(y)}")
            i += 4
    res = ""
    for part in out:
        if part.isalpha():
            res += part
        else:
            if res and not res[-1].isalpha():
                res += " "
            res += part
    return res


class Face:
    def __init__(self, weight):
        self.weight = weight
        self.glyphs = _GLYPHS_600 if weight == 600 else _GLYPHS_500
        self.cap_ratio = 27.68 / 40.0

    def run(self, text, size, x, baseline, tracking=0.0):
        """Devuelve (d, (xmin, ymin, xmax, ymax), avance) del texto ya interletrado."""
        scale = size / 40.0
        track_base = tracking * 40.0
        cursor = 0.0
        cmds = []
        xmin, ymin, xmax, ymax = float("inf"), float("inf"), float("-inf"), float("-inf")
        for i, ch in enumerate(text):
            gd, (gx0, gy0, gx1, gy1), adv = self.glyphs[ch]
            tx = x + cursor * scale
            ty = baseline
            cmds.append(_transform_svg_path(gd, scale, tx, ty))
            xmin = min(xmin, gx0 * scale + tx)
            ymin = min(ymin, gy0 * scale + ty)
            xmax = max(xmax, gx1 * scale + tx)
            ymax = max(ymax, gy1 * scale + ty)
            cursor += adv + (track_base if i < len(text) - 1 else 0.0)
        return "".join(cmds), (xmin, ymin, xmax, ymax), cursor * scale

    def width(self, text, size, tracking=0.0):
        _, b, _ = self.run(text, size, 0, 0, tracking)
        return b[2] - b[0], b[0]

    def cap_height(self, size):
        return self.cap_ratio * size


SEMIBOLD = Face(600)
MEDIUM = Face(500)

SUFFIX = "PDF"
NAME_TRACK = -0.01
SUFFIX_TRACK = 0.3


# --- Ensamblado -----------------------------------------------------------
class Doc:
    def __init__(self, uid, title, out_dir):
        self.uid = uid
        self.title = title
        self.out_dir = out_dir
        self.defs = []
        self.body = []
        self.box = [float("inf"), float("inf"), float("-inf"), float("-inf")]

    def grow(self, x0, y0, x1, y1):
        b = self.box
        self.box = [min(b[0], x0), min(b[1], y0), max(b[2], x1), max(b[3], y1)]

    def write(self, path, box=None, pad=0):
        x0, y0, x1, y1 = box or self.box
        x0, y0, x1, y1 = x0 - pad, y0 - pad, x1 + pad, y1 + pad
        w, h = x1 - x0, y1 - y0
        out = [
            f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="{fmt(x0)} {fmt(y0)} {fmt(w)} {fmt(h)}" '
            f'width="{fmt(w)}" height="{fmt(h)}" role="img" aria-labelledby="{self.uid}-t">',
            f'  <title id="{self.uid}-t">{self.title}</title>',
        ]
        if self.defs:
            out += ["  <defs>"] + [f"    {d}" for d in self.defs] + ["  </defs>"]
        out += [f"  {line}" for line in self.body]
        out.append("</svg>\n")
        full = os.path.join(self.out_dir, path)
        os.makedirs(os.path.dirname(full), exist_ok=True)
        with open(full, "w", encoding="utf-8", newline="\n") as f:
            f.write("\n".join(out))


def symbol(doc, p, x=0.0, y=0.0, scale=1.0, gap=GAP, b_box=None, knockout=""):
    """Dibuja el isotipo Prisma de Conversión Documental (Documento + Base + Vector).
    Si se especifica b_box=(bx, by, bw, bh), el zócalo base crece como contenedor (isologo).
    """
    mid_b = f"{doc.uid}-base"
    mid_a = f"{doc.uid}-doc"
    if b_box:
        bx, by, bw, bh = b_box
        mw, mh = max(SYMBOL, bx + bw) + 2, max(SYMBOL, by + bh) + 2
    else:
        mw, mh = SYMBOL + 2, SYMBOL + 2

    groove_w = 2.4 if gap == GAP else 3.0

    # Máscara del zócalo base / contenedor (b): recorta el pliego documental (DOC_A)
    # y el vector esmeralda (VECTOR_L) con canal de aire.
    doc.defs.append(
        f'<mask id="{mid_b}" maskUnits="userSpaceOnUse" x="-1" y="-1" width="{fmt(mw)}" height="{fmt(mh)}">'
        f'<rect x="-1" y="-1" width="{fmt(mw)}" height="{fmt(mh)}" fill="#fff"/>'
        f'<path d="{DOC_A}" fill="#000" stroke="#000" stroke-width="{gap}" stroke-linejoin="round"/>'
        f'<path d="{VECTOR_L}" fill="#000" stroke="#000" stroke-width="{gap}" stroke-linejoin="round"/>'
        f"{knockout}</mask>"
    )

    # Máscara del pliego documental (a): recorta la ventana de pliegue a 45° (FOLD),
    # las ranuras de datos (GROOVES), el zócalo base (BASE_B) y el vector (VECTOR_L).
    doc.defs.append(
        f'<mask id="{mid_a}" maskUnits="userSpaceOnUse" x="-1" y="-1" width="{fmt(mw)}" height="{fmt(mh)}">'
        f'<rect x="-1" y="-1" width="{fmt(mw)}" height="{fmt(mh)}" fill="#fff"/>'
        f'<path d="{FOLD}" fill="#000"/>'
        f'<path d="{GROOVES}" fill="none" stroke="#000" stroke-width="{fmt(groove_w)}" stroke-linecap="round"/>'
        f'<path d="{BASE_B}" fill="#000" stroke="#000" stroke-width="{gap}" stroke-linejoin="round"/>'
        f'<path d="{VECTOR_L}" fill="#000" stroke="#000" stroke-width="{gap}" stroke-linejoin="round"/></mask>'
    )

    transform = "" if (x, y, scale) == (0, 0, 1) else f' transform="translate({fmt(x)} {fmt(y)}){"" if scale == 1 else f" scale({fmt(scale)})"}"'
    body_lines = [
        f"<g{transform}>",
        f'  <g mask="url(#{mid_b})">',
    ]
    if b_box:
        bx, by, bw, bh = b_box
        body_lines.append(f'    <rect x="{fmt(bx)}" y="{fmt(by)}" width="{fmt(bw)}" height="{fmt(bh)}" rx="10" fill="{p["b"]}"/>')
    body_lines += [
        f'    <path d="{BASE_B}" fill="{p["b"]}"/>',
        "  </g>",
        f'  <g mask="url(#{mid_a})">',
        f'    <path d="{DOC_A}" fill="{p["a"]}"/>',
        "  </g>",
        f'  <path d="{VECTOR_L}" fill="{p["l"]}"/>',
        "</g>",
    ]
    doc.body += body_lines
    if b_box:
        bx, by, bw, bh = b_box
        doc.grow(x, y, x + max(SYMBOL, bx + bw) * scale, y + max(SYMBOL, by + bh) * scale)
    else:
        doc.grow(x, y, x + SYMBOL * scale, y + SYMBOL * scale)


def text(doc, face, s, size, x, baseline, fill, tracking=0.0):
    d, b, _ = face.run(s, size, x, baseline, tracking)
    doc.body.append(f'<path fill="{fill}" d="{d}"/>')
    doc.grow(*b)
    return b


def build_kit(name, out_dir):
    brand_title = f"{name} {SUFFIX}"

    def isotipo(variant):
        doc = Doc(f"isotipo-{variant}", f"{brand_title} — isotipo {LABEL[variant]}", out_dir)
        symbol(doc, PALETTES[variant])
        doc.write(f"isotipo/isotipo-{variant}.svg")

    def logotipo_horizontal(variant):
        p = PALETTES[variant]
        doc = Doc(f"logotipo-h-{variant}", f"{brand_title} — logotipo horizontal {LABEL[variant]}", out_dir)
        size = 40
        b = text(doc, SEMIBOLD, name, size, 0, 0, p["t"], NAME_TRACK)
        text(doc, MEDIUM, SUFFIX, size, b[2] + size * 0.28, 0, p["s"], 0.04)
        doc.write(f"logotipo/logotipo-horizontal-{variant}.svg")

    def logotipo_vertical(variant):
        p = PALETTES[variant]
        doc = Doc(f"logotipo-v-{variant}", f"{brand_title} — logotipo vertical {LABEL[variant]}", out_dir)
        size, small = 40, 16
        name_w, name_x0 = SEMIBOLD.width(name, size, NAME_TRACK)
        suf_w, suf_x0 = MEDIUM.width(SUFFIX, small, SUFFIX_TRACK)
        cx = name_w / 2
        text(doc, SEMIBOLD, name, size, -name_x0, 0, p["t"], NAME_TRACK)
        baseline = size * 0.2 + 16 + MEDIUM.cap_height(small)
        text(doc, MEDIUM, SUFFIX, small, cx - suf_w / 2 - suf_x0, baseline, p["s"], SUFFIX_TRACK)
        doc.write(f"logotipo/logotipo-vertical-{variant}.svg")

    def wordmark_stack(doc, p, x, center_y=None, top=None, align="left", cx=None, size=30, small=13):
        cap1, cap2, gap = SEMIBOLD.cap_height(size), MEDIUM.cap_height(small), size * 0.34
        block = cap1 + gap + cap2
        y0 = center_y - block / 2 if center_y is not None else top
        b1, b2 = y0 + cap1, y0 + block
        if align == "left":
            text(doc, SEMIBOLD, name, size, x, b1, p["t"], NAME_TRACK)
            text(doc, MEDIUM, SUFFIX, small, x, b2, p["s"], SUFFIX_TRACK)
        else:
            w1, o1 = SEMIBOLD.width(name, size, NAME_TRACK)
            w2, o2 = MEDIUM.width(SUFFIX, small, SUFFIX_TRACK)
            text(doc, SEMIBOLD, name, size, cx - w1 / 2 - o1, b1, p["t"], NAME_TRACK)
            text(doc, MEDIUM, SUFFIX, small, cx - w2 / 2 - o2, b2, p["s"], SUFFIX_TRACK)
        return block

    def imagotipo_horizontal(variant):
        p = PALETTES[variant]
        doc = Doc(f"imagotipo-h-{variant}", f"{brand_title} — imagotipo horizontal {LABEL[variant]}", out_dir)
        symbol(doc, p)
        sz = 30
        wordmark_stack(doc, p, SYMBOL + 14, center_y=SYMBOL / 2, size=sz, small=15)
        doc.write(f"imagotipo/imagotipo-horizontal-{variant}.svg")

    def imagotipo_vertical(variant):
        p = PALETTES[variant]
        doc = Doc(f"imagotipo-v-{variant}", f"{brand_title} — imagotipo vertical {LABEL[variant]}", out_dir)
        size = 30
        name_w, _ = SEMIBOLD.width(name, size, NAME_TRACK)
        cx = max(name_w, SYMBOL * 1.5) / 2
        symbol(doc, p, x=cx - SYMBOL * 0.75, scale=1.5)
        wordmark_stack(doc, p, 0, top=SYMBOL * 1.5 + 20, align="center", cx=cx, size=size, small=15)
        doc.write(f"imagotipo/imagotipo-vertical-{variant}.svg")

    def isologo(variant, layout):
        p = ISOLOGO[variant]
        doc = Doc(f"isologo-{layout[0]}-{variant}", f"{brand_title} — isologo {layout} {LABEL[variant]}", out_dir)
        lines = []
        if layout == "horizontal":
            size, small = 26, 13
            cap1, cap2 = SEMIBOLD.cap_height(size), MEDIUM.cap_height(small)
            gap = size * 0.34
            x = SYMBOL + 10
            b1 = 52 - (cap1 + gap + cap2) / 2 + cap1
            lines += [
                (SEMIBOLD, name, size, x, b1, "t", NAME_TRACK),
                (MEDIUM, SUFFIX, small, x, b1 + gap + cap2, "s", SUFFIX_TRACK),
            ]
            w, _ = SEMIBOLD.width(name, size, NAME_TRACK)
            b_box = (20, 20, x + w + 24 - 20, 64)
        else:
            size, small = 28, 14
            x = 38
            b1 = SYMBOL + 14 + SEMIBOLD.cap_height(size)
            b2 = b1 + size * 0.38 + MEDIUM.cap_height(small)
            lines += [
                (SEMIBOLD, name, size, x, b1, "t", NAME_TRACK),
                (MEDIUM, SUFFIX, small, x, b2, "s", SUFFIX_TRACK),
            ]
            w, _ = SEMIBOLD.width(name, size, NAME_TRACK)
            b_box = (20, 20, x + w + 24 - 20, b2 + 20 - 20)

        if variant == "mono":
            knock = "".join(f'<path d="{f.run(s, sz, lx, bl, tr)[0]}" fill="#000"/>' for f, s, sz, lx, bl, _, tr in lines)
            symbol(doc, p, b_box=b_box, knockout=knock)
        else:
            symbol(doc, p, b_box=b_box)
            for f, s, sz, lx, bl, role, tr in lines:
                text(doc, f, s, sz, lx, bl, p[role], tr)
        doc.write(f"isologo/isologo-{layout}-{variant}.svg", box=(0, 0, 20 + b_box[2], 20 + b_box[3]))

    def tile(folder, fname, variant, side, scale, radius, title):
        bg, p = TILES[variant]
        doc = Doc(f"{fname}-{variant}", title, out_dir)
        doc.body.append(f'<rect width="{side}" height="{side}"{f" rx={chr(34)}{radius}{chr(34)}" if radius else ""} fill="{bg}"/>')
        offset = side / 2 - SYMBOL * scale / 2
        symbol(doc, p, x=offset, y=offset, scale=scale, gap=GAP if side > 64 else GAP_SMALL)
        doc.write(f"{folder}/{fname}-{variant}.svg", box=(0, 0, side, side))

    def favicon():
        light, dark = PALETTES["color"], PALETTES["negativo"]
        doc = Doc("favicon", brand_title, out_dir)
        doc.defs.append(
            "<style>"
            f".a{{fill:{light['a']}}}.b{{fill:{light['b']}}}.l{{fill:{light['l']}}}"
            f"@media (prefers-color-scheme:dark){{.a{{fill:{dark['a']}}}.b{{fill:{dark['b']}}}.l{{fill:{dark['l']}}}}}"
            "</style>"
        )
        doc.defs.append(
            f'<mask id="favicon-base" maskUnits="userSpaceOnUse" x="-1" y="-1" width="58" height="58">'
            f'<rect x="-1" y="-1" width="58" height="58" fill="#fff"/>'
            f'<path d="{DOC_A}" fill="#000" stroke="#000" stroke-width="{GAP_SMALL}" stroke-linejoin="round"/>'
            f'<path d="{VECTOR_L}" fill="#000" stroke="#000" stroke-width="{GAP_SMALL}" stroke-linejoin="round"/></mask>'
        )
        doc.defs.append(
            f'<mask id="favicon-doc" maskUnits="userSpaceOnUse" x="-1" y="-1" width="58" height="58">'
            f'<rect x="-1" y="-1" width="58" height="58" fill="#fff"/>'
            f'<path d="{FOLD}" fill="#000"/>'
            f'<path d="{GROOVES}" fill="none" stroke="#000" stroke-width="3.0" stroke-linecap="round"/>'
            f'<path d="{BASE_B}" fill="#000" stroke="#000" stroke-width="{GAP_SMALL}" stroke-linejoin="round"/>'
            f'<path d="{VECTOR_L}" fill="#000" stroke="#000" stroke-width="{GAP_SMALL}" stroke-linejoin="round"/></mask>'
        )
        doc.body += [
            '<g mask="url(#favicon-base)">',
            f'  <path class="b" d="{BASE_B}"/>',
            "</g>",
            '<g mask="url(#favicon-doc)">',
            f'  <path class="a" d="{DOC_A}"/>',
            "</g>",
            f'<path class="l" d="{VECTOR_L}"/>',
        ]
        doc.write("favicon/favicon.svg", box=(0, 0, SYMBOL, SYMBOL))

    for v in PALETTES:
        isotipo(v)
        logotipo_horizontal(v)
        logotipo_vertical(v)
        imagotipo_horizontal(v)
        imagotipo_vertical(v)
        isologo(v, "horizontal")
        isologo(v, "vertical")

    favicon()
    for v in TILES:
        tile("app-icon", "app-icon", v, 512, 5, 112, f"{brand_title} — icono de aplicación {v}")
        tile("redes", "avatar", v, 400, 4, 0, f"{brand_title} — avatar {v}")
        tile("favicon", "favicon-32", v, 32, 0.5, 7, f"{brand_title} — favicon {v}")


if __name__ == "__main__":
    root_out = sys.argv[2] if len(sys.argv) > 2 else (sys.argv[1] if len(sys.argv) > 1 else ".")
    build_kit("XPress", root_out)
    print("XPress PDF Brand Kit generated successfully.")
