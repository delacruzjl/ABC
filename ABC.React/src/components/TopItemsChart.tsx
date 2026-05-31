import React from "react"
import { useTranslation } from "react-i18next"
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  Cell,
} from "recharts"
import type { ItemWithObservationCount } from "../types/observation"

interface Props {
  title: string
  items: ItemWithObservationCount[]
  barColor?: string
}

const COLORS = [
  "#22d3ee",
  "#06b6d4",
  "#0891b2",
  "#0e7490",
  "#155e75",
  "#164e63",
  "#083344",
  "#67e8f9",
  "#a5f3fc",
  "#cffafe",
]

export const TopItemsChart: React.FC<Props> = ({
  title,
  items,
  barColor = "#22d3ee",
}) => {
  const { t } = useTranslation()

  if (items.length === 0) {
    return (
      <div className="bg-slate-700 rounded-lg p-4">
        <h3 className="text-lg font-semibold text-cyan-400 mb-3">{title}</h3>
        <p className="text-slate-400 text-sm">{t("dashboard.noDataAvailable")}</p>
      </div>
    )
  }

  const chartData = items.map((item) => ({
    name: item.name.length > 15 ? item.name.slice(0, 15) + "…" : item.name,
    fullName: item.name,
    count: item.observationCount,
  }))

  return (
    <div className="bg-slate-700 rounded-lg p-4">
      <h3 className="text-lg font-semibold text-cyan-400 mb-3">{title}</h3>
      <ResponsiveContainer width="100%" height={300}>
        <BarChart
          data={chartData}
          margin={{ top: 5, right: 20, left: 0, bottom: 60 }}
        >
          <CartesianGrid strokeDasharray="3 3" stroke="#475569" />
          <XAxis
            dataKey="name"
            tick={{ fill: "#cbd5e1", fontSize: 11 }}
            angle={-45}
            textAnchor="end"
            height={80}
          />
          <YAxis
            tick={{ fill: "#cbd5e1", fontSize: 12 }}
            allowDecimals={false}
          />
          <Tooltip
            contentStyle={{
              backgroundColor: "#1e293b",
              border: "1px solid #475569",
              borderRadius: "8px",
              color: "#e2e8f0",
            }}
            formatter={(value: number) => [value, t("common.observations")]}
            labelFormatter={(_label, payload) =>
              payload?.[0]?.payload?.fullName ?? _label
            }
          />
          <Bar dataKey="count" radius={[4, 4, 0, 0]}>
            {chartData.map((_entry, index) => (
              <Cell
                key={`cell-${index}`}
                fill={COLORS[index % COLORS.length] || barColor}
              />
            ))}
          </Bar>
        </BarChart>
      </ResponsiveContainer>
    </div>
  )
}
