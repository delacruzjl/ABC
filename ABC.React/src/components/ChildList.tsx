import React from "react"
import { useTranslation } from "react-i18next"
import { ChildCard } from "./ChildCard"
import type { Child } from "../types/child"

interface Props {
  children: Child[]
  onAdd: () => void
  onEdit?: (id: string) => void
  onDelete: (id: string) => void
  defaultChildId?: string | null
  onSetDefault?: (id: string | null) => void | Promise<unknown>
}

export const ChildList: React.FC<Props> = ({
  children: childList,
  onAdd,
  onEdit,
  onDelete,
  defaultChildId,
  onSetDefault,
}) => {
  const { t } = useTranslation()

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-cyan-400">{t("children.title")}</h1>
          <p className="text-slate-400 text-sm mt-1">
            {t("children.manageDescription")}
          </p>
        </div>
        <button
          onClick={onAdd}
          className="bg-cyan-600 hover:bg-cyan-500 text-white font-medium px-4 py-2 rounded-lg transition"
        >
          + {t("children.addChild")}
        </button>
      </div>

      {childList.length === 0 ? (
        <div className="text-center py-12">
          <p className="text-slate-400">{t("children.noChildren")}</p>
        </div>
      ) : (
        <div className="flex flex-col gap-3">
          {childList.map((child) => (
            <ChildCard
              key={child.id}
              child={child}
              onEdit={onEdit}
              onDelete={onDelete}
              isDefault={defaultChildId === child.id}
              onSetDefault={onSetDefault}
            />
          ))}
        </div>
      )}
    </div>
  )
}
