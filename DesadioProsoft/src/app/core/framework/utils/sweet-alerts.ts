import swal, { SweetAlertIcon } from 'sweetalert2';

export function fire(titulo: string, text: string, icon: SweetAlertIcon) {
  swal.fire({
    icon: icon,
    title: titulo,
    text: titulo,
    customClass: {
      confirmButton: 'btn btn-success'
    },
  })
}

export function Confirma(
  title: string,
  text: string,
  icon: SweetAlertIcon,
  callback: Function
) {
  swal.fire({
    title: title,
    text: text,
    icon: icon,
    showCancelButton: true,
    confirmButtonColor: '#2F8BE6',
    cancelButtonColor: '#F55252',
    confirmButtonText: '&nbsp;Sim&nbsp;',
    customClass: {
      confirmButton: 'btn btn-info',
      cancelButton: 'btn btn-warning ml-1'
    },
    buttonsStyling: false,
  }).then(function (result) {
    callback(result)
  });
}
