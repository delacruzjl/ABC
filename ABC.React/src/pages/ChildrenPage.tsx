import React, { useState } from "react"
import { useNavigate } from "react-router-dom"
import { useTranslation } from "react-i18next"
import { useChildren, useDefaultChild } from "../hooks/useChildren"
import { ChildList } from "../components/ChildList"

export const ChildrenPage: React.FC = () => {
  const { t } = useTranslation()
  const { children, loading, error, removeChild } = useChildren()
  const { defaultChildId, setDefaultChild } = useDefaultChild()
  const [actionError, setActionError] = useState<string | null>(null)
  const navigate = useNavigate()

  const handleSetDefault = async (id: string | null) => {
    setActionError(null)
    try {
      const result = await setDefaultChild(id)
      if (result.errors?.length) {
        setActionError(result.errors[0].message)
      }
    } catch (e: unknown) {
      setActionError(
        e instanceof Error ? e.message : t("children.failedSetDefault")
      )
    }
  }

  if (loading) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-cyan-400 text-lg">{t("children.loading")}</span>
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
    <div>
      {actionError && (
        <div className="mb-4 p-3 bg-red-900/50 border border-red-700 rounded-lg text-red-300 text-sm flex justify-between items-center">
          <span>{actionError}</span>
          <button
            onClick={() => setActionError(null)}
            className="text-red-400 hover:text-red-200 ml-4"
            aria-label={t("common.dismiss")}
          >
            ✕
          </button>
        </div>
      )}
      <ChildList
        children={children}
        onAdd={() => navigate("/child/manage")}
        onEdit={(id) => navigate(`/child/edit/${id}`)}
        onDelete={(id) => removeChild(id)}
        defaultChildId={defaultChildId}
        onSetDefault={handleSetDefault}
      />
    </div>
  )
}
