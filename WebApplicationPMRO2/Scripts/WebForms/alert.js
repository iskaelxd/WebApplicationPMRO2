
// level: “primary”, “success”, “warning”, “danger”, etc.
// pos: “top-0 end-0”, “bottom-0 start-0”, … cualquiera válido de Bootstrap
window.mroShowToast = function (message, level, pos, delay) {

    // contenedor fijo (lo crea la primera vez)
    let area = document.getElementById('mro-toast-area');
    if (!area) {
        area = document.createElement('div');
        area.id = 'mro-toast-area';
        area.className = 'toast-container position-fixed p-3 ' + pos;
        area.style.zIndex = 1100;
        document.body.appendChild(area);
    } else {
        // cambia posición si la llamada la pide diferente
        area.className = 'toast-container position-fixed p-3 ' + pos;
    }

    // crea toast
    const id = 'toast-' + crypto.randomUUID();
    const div = document.createElement('div');
    div.id = id;
    div.className = `toast text-white bg-${level} border-0`;
    div.innerHTML = `
          <div class="d-flex">
              <div class="toast-body">${message}</div>
              <button class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
          </div>`;
    area.appendChild(div);

    // muestra
    const t = new bootstrap.Toast(div, { delay: delay ?? 4000 });
    t.show();
};

