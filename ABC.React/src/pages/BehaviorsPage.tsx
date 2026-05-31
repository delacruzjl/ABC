import React from "react"
import { useNavigate } from "react-router-dom"
import { useTranslation } from "react-i18next"
import { useBehaviors } from "../hooks/useBehaviors"
import { BehaviorList } from "../components/BehaviorList"

export const BehaviorsPage: React.FC = () => {
  const { t } = useTranslation()
  const { behaviors, loading, error, removeBehavior } = useBehaviors()
  const navigate = useNavigate()

  if (loading) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-cyan-400 text-lg">{t("behaviors.loading")}</span>
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
    <BehaviorList
      behaviors={behaviors}
      onAdd={() => navigate("/behavior/manage")}
      onEdit={(id) => navigate(`/behavior/manage?id=${id}`)}
      onDelete={(id) => removeBehavior(id)}
    />
  )
}
