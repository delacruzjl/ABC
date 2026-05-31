import React from "react"
import { useNavigate } from "react-router-dom"
import { useTranslation } from "react-i18next"
import { useConsequences } from "../hooks/useConsequences"
import { ConsequenceList } from "../components/ConsequenceList"

export const ConsequencesPage: React.FC = () => {
  const { t } = useTranslation()
  const { consequences, loading, error, removeConsequence } = useConsequences()
  const navigate = useNavigate()

  if (loading) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-cyan-400 text-lg">{t("consequences.loading")}</span>
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
    <ConsequenceList
      consequences={consequences}
      onAdd={() => navigate("/consequence/manage")}
      onEdit={(id) => navigate(`/consequence/manage?id=${id}`)}
      onDelete={(id) => removeConsequence(id)}
    />
  )
}
