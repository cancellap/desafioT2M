import React from "react";
import { Formik, Field, Form, ErrorMessage } from "formik";
import * as Yup from "yup";
import { addTarefa } from "../../Service/Api";
import styles from "./ModalAddTarefa.module.css";

const validationSchema = Yup.object({
  nome: Yup.string().required("Nome da tarefa é obrigatório"),
  descricao: Yup.string().required("Descrição é obrigatória"),
  prazo: Yup.date().required("Prazo é obrigatório").nullable(),
  statusTarefa: Yup.number()
    .required("Status da tarefa é obrigatório")
    .oneOf([0, 1, 2], "Status inválido"),
});

export default function ModalAddTarefa({ projetoId, onClose, onSave }) {
  const initialValues = {
    nome: "",
    descricao: "",
    prazo: "",
    statusTarefa: null,
  };

  const getStatusTarefaValue = (status) => {
    switch (status) {
      case "NaoIniciado":
        return 0;
      case "EmAndamento":
        return 1;
      case "Concluida":
        return 2;
      default:
        return 0;
    }
  };
  const handleSave = async (values) => {
    const novaTarefa = {
      nome: values.nome,
      descricao: values.descricao,
      prazo: values.prazo,
      statusTarefa: getStatusTarefaValue(values.statusTarefa),
      usuarioId: values.usuarioId,
      projetoId: projetoId,
    };

    const token = localStorage.getItem("token");
    if (token) {
      try {
        const tarefaCriada = await addTarefa(novaTarefa, token);
        console.log("Tarefa criada com sucesso:", tarefaCriada);
        onSave();
        onClose();
      } catch (error) {
        console.error("Erro ao salvar a tarefa:", error);
      }
    } else {
      console.log("Token não encontrado");
    }
  };

  return (
    <div className={styles.modalOverlay}>
      <div className={styles.modalContent}>
        <h2>Adicionar Tarefa</h2>
        <Formik
          initialValues={initialValues}
          validationSchema={validationSchema}
          onSubmit={handleSave}
        >
          <Form>
            <div className={styles.label}>
              <label>Nome da Tarefa:</label>
              <Field className={styles.input} type="text" name="nome" />
              <ErrorMessage
                name="nome"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.label}>
              <label>Descrição:</label>
              <Field className={styles.input} type="text" name="descricao" />
              <ErrorMessage
                name="descricao"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.label}>
              <label>Prazo:</label>
              <Field className={styles.input} type="date" name="prazo" />
              <ErrorMessage
                name="prazo"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.label}>
              <label>Status da Tarefa:</label>
              <Field as="select" className={styles.select} name="statusTarefa">
                <option value={null}>Selecione</option>
                <option value={0}>Não Iniciada</option>
                <option value={1}>Em Andamento</option>
              </Field>
              <ErrorMessage
                name="statusTarefa"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.buttons}>
              <button
                className={styles.cancelButton}
                type="button"
                onClick={onClose}
              >
                Cancelar
              </button>
              <button className={styles.button} type="submit">
                Adicionar
              </button>
            </div>
          </Form>
        </Formik>
      </div>
    </div>
  );
}
