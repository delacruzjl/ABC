import React from "react"
import { useNavigate } from "react-router-dom"
import { useTranslation } from "react-i18next"
import { useDashboard } from "../hooks/useDashboard"
import { TopItemsChart } from "../components/TopItemsChart"
import { ChildObservationChart } from "../components/ChildObservationChart"
import { RecentObservationsList } from "../components/RecentObservationsList"
import { useAuth } from "../context/AuthContext"

export const DashboardPage: React.FC = () => {
  const { t } = useTranslation()
  const {
    topAntecedents,
    topBehaviors,
    topConsequences,
    recentObservations,
    loading,
    error,
  } = useDashboard()
  const navigate = useNavigate()
  const { isAdmin } = useAuth()

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-cyan-400 mb-2">
          {t("dashboard.title")}
        </h1>
        <p className="text-slate-400">{t("dashboard.overview")}</p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-8">
        <button
          onClick={() => navigate("/children")}
          className="bg-slate-700 hover:bg-slate-600 rounded-lg p-4 text-left transition border border-slate-600 hover:border-cyan-400"
        >
          <h3 className="text-lg font-semibold text-cyan-400">
            {t("nav.children")}
          </h3>
          <p className="text-slate-400 text-sm mt-1">
            {t("dashboard.childrenDescription")}
          </p>
        </button>
        {isAdmin && (
          <>
            <button
              onClick={() => navigate("/antecedents")}
              className="bg-slate-700 hover:bg-slate-600 rounded-lg p-4 text-left transition border border-slate-600 hover:border-cyan-400"
            >
              <h3 className="text-lg font-semibold text-cyan-400">
                {t("nav.antecedents")}
              </h3>
              <p className="text-slate-400 text-sm mt-1">
                {t("dashboard.antecedentsDescription")}
              </p>
            </button>
            <button
              onClick={() => navigate("/behaviors")}
              className="bg-slate-700 hover:bg-slate-600 rounded-lg p-4 text-left transition border border-slate-600 hover:border-cyan-400"
            >
              <h3 className="text-lg font-semibold text-cyan-400">
                {t("nav.behaviors")}
              </h3>
              <p className="text-slate-400 text-sm mt-1">
                {t("dashboard.behaviorsDescription")}
              </p>
            </button>
            <button
              onClick={() => navigate("/consequences")}
              className="bg-slate-700 hover:bg-slate-600 rounded-lg p-4 text-left transition border border-slate-600 hover:border-cyan-400"
            >
              <h3 className="text-lg font-semibold text-cyan-400">
                {t("nav.consequences")}
              </h3>
              <p className="text-slate-400 text-sm mt-1">
                {t("dashboard.consequencesDescription")}
              </p>
            </button>
          </>
        )}
      </div>

      {loading && (
        <div className="flex justify-center py-12">
          <span className="text-cyan-400 text-lg">{t("dashboard.loading")}</span>
        </div>
      )}

      {error && (
        <div className="flex justify-center py-4 mb-4">
          <span className="text-red-400 text-sm">
            {t("common.error")}: {error.message}
          </span>
        </div>
      )}

      {!loading && (
        <>
          <h2 className="text-xl font-bold text-slate-200 mb-4">
            {t("dashboard.topByObservationCount")}
          </h2>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-4 mb-8">
            <TopItemsChart
              title={t("dashboard.topAntecedents")}
              items={topAntecedents}
              barColor="#22d3ee"
            />
            <TopItemsChart
              title={t("dashboard.topBehaviors")}
              items={topBehaviors}
              barColor="#a78bfa"
            />
            <TopItemsChart
              title={t("dashboard.topConsequences")}
              items={topConsequences}
              barColor="#fbbf24"
            />
          </div>

          <h2 className="text-xl font-bold text-slate-200 mb-4">
            {t("dashboard.childrenObservationsPerItem")}
          </h2>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-4 mb-8">
            <ChildObservationChart
              title={t("dashboard.topAntecedents")}
              items={topAntecedents}
              barColor="#22d3ee"
            />
            <ChildObservationChart
              title={t("dashboard.topBehaviors")}
              items={topBehaviors}
              barColor="#a78bfa"
            />
            <ChildObservationChart
              title={t("dashboard.topConsequences")}
              items={topConsequences}
              barColor="#fbbf24"
            />
          </div>

          <h2 className="text-xl font-bold text-slate-200 mb-4">
            {t("dashboard.mostRecentThree")}
          </h2>
          <RecentObservationsList observations={recentObservations} />
        </>
      )}
    </div>
  )
}
