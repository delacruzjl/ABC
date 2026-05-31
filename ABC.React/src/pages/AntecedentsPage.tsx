import React from "react"
import { useNavigate } from "react-router-dom"
import { useTranslation } from "react-i18next"
import { useAntecedents } from "../hooks/useAntecedents"
import { AntecedentList } from "../components/AntecedentList"

export const AntecedentsPage: React.FC = () => {
  const { t } = useTranslation()
  const { antecedents, loading, error, removeAntecedent } = useAntecedents()
  const navigate = useNavigate()

  if (loading) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-cyan-400 text-lg">{t("antecedents.loading")}</span>
      </div>
    )
  }

  if (error) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-red-400 text-lg">
          {t("common.error")}: {error.message}
        </span>
      </div>
    )
  }

  return (
    <AntecedentList
      antecedents={antecedents}
      onAdd={() => navigate("/antecedent/manage")}
      onEdit={(id) => navigate(`/antecedent/manage?id=${id}`)}
      onDelete={(id) => removeAntecedent(id)}
    />
  )
}
