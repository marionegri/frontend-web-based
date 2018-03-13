jQuery.support.cors = true;

function formatDate(date, format) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    if (format == 'yyyy-mm-dd') {
        return [year, month, day].join('-');
    }
    else {
        return [day, month, year].join('/');
    }    
}

$(document).ready(function () {
    PreencherTabela(null);

    $("#formCadastro").submit(function (e) {
        e.preventDefault();

        CadastrarPessoa();
    });

    $('#formEditar').submit(function (e) {
        e.preventDefault();

        EditarPessoa();
    });

    GraficoFaixa();

    GraficoSexo();
});

function GraficoSexo() {

    var resultSexo = [];

    $.ajax({
        url: 'http://localhost:8000/qtd_sexos',
        type: 'GET',
        crossDomain: true,
        dataType: 'json',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                resultSexo.push({ y: data[i].qtde, label: data[i].descricao });
            }

            var optionsSexo = {
                title: {
                    text: "Qtde. Sexo"
                },
                animationEnabled: true,
                data: [{
                    type: "pie",
                    startAngle: 40,
                    showInLegend: "true",
                    legendText: "{label}",
                    indexLabelFontSize: 16,
                    dataPoints: resultSexo
                }]
            };

            $("#reportSexo").CanvasJSChart(optionsSexo);
        },
        error: function (data) {
            FailureCallBack(data);
        }
    });
}

function GraficoFaixa() {

    var resultFaixa = [];

    $.ajax({
        url: 'http://localhost:8000/faixa_idades',
        type: 'GET',
        crossDomain: true,
        dataType: 'json',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                resultFaixa.push({ y: data[i].qtde, label: data[i].descricao });
            }

            var optionsFaixa = {
                title: {
                    text: "Faixa de Idades"
                },
                animationEnabled: true,
                data: [{
                    type: "pie",
                    startAngle: 40,
                    showInLegend: "true",
                    legendText: "{label}",
                    indexLabelFontSize: 16,
                    dataPoints: resultFaixa
                }]
            };

            $("#reportFaixa").CanvasJSChart(optionsFaixa);
        },
        error: function (data) {
            FailureCallBack(data);
        }
    });
}

function PreencherTabela(nome) {

    var getUrl = 'http://localhost:8000/pessoas';

    if (nome != null) {
        getUrl += '?nome=' + nome;
    }

    $('#dadosPessoas').empty();
    $.ajax({
        type: 'GET',
        crossDomain: true,
        dataType: 'json',	//Definimos o tipo de retorno
        url: getUrl,
        success: function (dados) {
            for (var i = 0; dados.dataLista.length > i; i++) {
                //Adicionando registros retornados na tabela
                $('#dadosPessoas').append('<tr><td>' + dados.dataLista[i].id + '</td>'
                    + '<td>' + dados.dataLista[i].nome + '</td>'
                    + '<td>' + formatDate(dados.dataLista[i].dataNascimento, 'dd/mm/yyyy') + '</td>'
                    + '<td>' + dados.dataLista[i].documento + '</td>'
                    + '<td>' + dados.dataLista[i].sexo + '</td>'
                    + '<td>' + dados.dataLista[i].endereco + '</td>'
                    + '<td class="center"><a onclick="ViewPessoa(' + dados.dataLista[i].id + ');" href="#modalEdit" role="button" class="btn btn-success btn-default" data-toggle="modal"><i class="fas fa-edit"></i></a></td>'
                    + '<td class="center"><button onclick="DeletePessoa(' + dados.dataLista[i].id + ');" type="button" class="btn btn-danger btn-default"><i class="fas fa-trash-alt"></i></button></td></tr>');
            }
        }
    });
}

$('#btnPesquisar').click(function () {
    PreencherTabela($('#txtPesquisa').val());
});

function EditarPessoa() {
    var json = {
        Nome: $('#txtNomeEditar').val(),
        DataNascimento: $('#txtDtNascimentoEditar').val(),
        Documento: $('#txtDocumentoEditar').val(),
        Sexo: $('#txtSexoEditar').val(),
        Endereco: $('#txtEnderecoEditar').val()
    };

    $.ajax({
        url: 'http://localhost:8000/pessoas/' + $('#hdPessoa').val(),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        type: 'PUT',
        crossDomain: true,
        dataType: 'json',
        data: JSON.stringify(json),
        success: function (data) {
            $('#btnCancelarEdit').click();
            PreencherTabela(null);
        },
        error: function (data) {
            FailureCallBack(data);
        }
    });
}

function CadastrarPessoa() {
    var json = {
        Nome: $('#txtNome').val(),
        DataNascimento: $('#txtDtNascimento').val(),
        Documento: $('#txtDocumento').val(),
        Sexo: $('#txtSexo').val(),
        Endereco: $('#txtEndereco').val()
    };

    $.ajax({
        url: 'http://localhost:8000/pessoas',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        type: 'POST',
        crossDomain: true,
        dataType: 'json',
        data: JSON.stringify(json),
        success: function (data) {
            $('#btnCancelarCadastro').click();
            PreencherTabela(null);
        },
        error: function (data) {
            FailureCallBack(data);
        }
    });
}

function DeletePessoa(id) {
    $.ajax({
        url: 'http://localhost:8000/pessoas/' + id,
        type: 'DELETE',
        crossDomain: true,
        dataType: 'json',
        success: function (data) {
            PreencherTabela(null);
        },
        error: function (data) {
            FailureCallBack(data);
        }
    });
}

function ViewPessoa(id) {
    $('#hdPessoa').val(id);
    $.ajax({
        url: 'http://localhost:8000/pessoas/' + id,
        type: 'GET',
        crossDomain: true,
        dataType: 'json',
        success: function (data) {
            $('#txtNomeEditar').val(data.data.nome);
            $('#txtDtNascimentoEditar').val(formatDate(data.data.dataNascimento, 'yyyy-mm-dd'));
            $('#txtDocumentoEditar').val(data.data.documento);
            $('#txtSexoEditar').val(data.data.sexo);
            $('#txtEnderecoEditar').val(data.data.endereco);
        },
        error: function (data) {
            FailureCallBack(data);
        }
    });
}

$("#btnSearch").click(function () {
    $.ajax({
        url: 'http://localhost:8000/pessoas',
        type: 'GET',
        crossDomain: true,
        dataType: 'json',
        success: function (data) {
            SucessCallback(data);
        },
        error: function (data) {
            FailureCallBack(data);
        }
    });
});

function SucessCallback(result) {
    alert(result);
}

function FailureCallBack(result) {
    alert("ERRO: " + result.status + ' ' + result.statusText);
}