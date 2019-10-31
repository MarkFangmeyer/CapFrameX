﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CapFrameX.Contracts.Configuration;
using CapFrameX.Contracts.Data;
using CapFrameX.EventAggregation.Messages;
using CapFrameX.Extensions;
using CapFrameX.OcatInterface;
using CapFrameX.Statistics;
using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace CapFrameX.ViewModel
{
	public class ReportViewModel : BindableBase, INavigationAware, IDropTarget
	{
		private readonly IStatisticProvider _frametimeStatisticProvider;
		private readonly IEventAggregator _eventAggregator;
		private readonly IAppConfiguration _appConfiguration;

		private bool _useEventMessages;

		public ObservableCollection<ReportInfo> ReportInfoCollecion { get; }
			= new ObservableCollection<ReportInfo>();

		public ICommand CopyTableDataCommand { get; }

		public ReportViewModel(IStatisticProvider frametimeStatisticProvider,
							  IEventAggregator eventAggregator,
							  IAppConfiguration appConfiguration)
		{
			_frametimeStatisticProvider = frametimeStatisticProvider;
			_eventAggregator = eventAggregator;
			_appConfiguration = appConfiguration;

			CopyTableDataCommand = new DelegateCommand(OnCopyTableData);

			SubscribeToSelectRecord();
		}

		private void OnCopyTableData()
		{
			if (!ReportInfoCollecion.Any())
				return;

			StringBuilder builder = new StringBuilder();

			// Header
			var displayNameGame = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.Game);
			var displayNameDate = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.Date);
			var displayNameTime = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.Time);
			var displayNameNumberOfSamples = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.NumberOfSamples);
			var displayNameRecordTime = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.RecordTime);
			var displayNameCpu = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.Cpu);
			var displayNameGraphicCard = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.GraphicCard);
			var displayNameMaxFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.MaxFps);
			var displayNameNinetyNinePercentQuantileFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.NinetyNinePercentQuantileFps);
			var displayNameNinetyFivePercentQuantileFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.NinetyFivePercentQuantileFps);
			var displayNameAverageFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.AverageFps);
			var displayNameFivePercentQuantileFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.FivePercentQuantileFps);
			var displayNameOnePercentQuantileFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.OnePercentQuantileFps);
			var displayNameOnePercentLowAverageFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.OnePercentLowAverageFps);
            var displayNameZeroDotTwoPercentQuantileFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.ZeroDotTwoPercentQuantileFps);
            var displayNameZeroDotOnePercentQuantileFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.ZeroDotOnePercentQuantileFps);
			var displayNameZeroDotOnePercentLowAverageFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.ZeroDotOnePercentLowAverageFps);
			var displayNameMinFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.MinFps);
			var displayNameAdaptiveSTDFps = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.AdaptiveSTDFps);
			var displayNameCustomComment = ReflectionExtensions.GetPropertyDisplayName<ReportInfo>(x => x.CustomComment);

			builder.Append(displayNameGame + "\t" +
						   displayNameDate + "\t" +
						   displayNameTime + "\t" +
						   displayNameNumberOfSamples + "\t" +
						   displayNameRecordTime + "\t" +
						   displayNameCpu + "\t" +
						   displayNameGraphicCard + "\t" +
						   displayNameMaxFps + "\t" +
						   displayNameNinetyNinePercentQuantileFps + "\t" +
						   displayNameNinetyFivePercentQuantileFps + "\t" +
						   displayNameAverageFps + "\t" +
						   displayNameFivePercentQuantileFps + "\t" +
						   displayNameOnePercentQuantileFps + "\t" +
						   displayNameOnePercentLowAverageFps + "\t" +
                           displayNameZeroDotTwoPercentQuantileFps + "\t" +
                           displayNameZeroDotOnePercentQuantileFps + "\t" +
						   displayNameZeroDotOnePercentLowAverageFps + "\t" +
						   displayNameMinFps + "\t" +
						   displayNameAdaptiveSTDFps + "\t" +
						   displayNameCustomComment +
						   Environment.NewLine);

			foreach (var reportInfo in ReportInfoCollecion)
			{
				builder.Append(reportInfo.Game + "\t" +
							   reportInfo.Date + "\t" +
							   reportInfo.Time + "\t" +
							   reportInfo.NumberOfSamples + "\t" +
							   reportInfo.RecordTime + "\t" +
							   reportInfo.Cpu + "\t" +
							   reportInfo.GraphicCard + "\t" +
							   reportInfo.MaxFps.ToString(CultureInfo.InvariantCulture) + "\t" +
							   reportInfo.NinetyNinePercentQuantileFps.ToString(CultureInfo.InvariantCulture) + "\t" +
							   reportInfo.NinetyFivePercentQuantileFps.ToString(CultureInfo.InvariantCulture) + "\t" +
							   reportInfo.AverageFps.ToString(CultureInfo.InvariantCulture) + "\t" +
							   reportInfo.FivePercentQuantileFps.ToString(CultureInfo.InvariantCulture) + "\t" +
							   reportInfo.OnePercentQuantileFps.ToString(CultureInfo.InvariantCulture) + "\t" +
							   reportInfo.OnePercentLowAverageFps.ToString(CultureInfo.InvariantCulture) + "\t" +
                               reportInfo.ZeroDotTwoPercentQuantileFps.ToString(CultureInfo.InvariantCulture) + "\t" +
							   reportInfo.ZeroDotOnePercentQuantileFps.ToString(CultureInfo.InvariantCulture) + "\t" +
                               reportInfo.ZeroDotOnePercentLowAverageFps.ToString(CultureInfo.InvariantCulture) + "\t" +
							   reportInfo.MinFps.ToString(CultureInfo.InvariantCulture) + "\t" +
							   reportInfo.AdaptiveSTDFps.ToString(CultureInfo.InvariantCulture) + "\t" +
							   reportInfo.CustomComment +
							   Environment.NewLine);
			}

			Clipboard.SetDataObject(builder.ToString(), false);
		}

		private void SubscribeToSelectRecord()
		{
			_eventAggregator.GetEvent<PubSubEvent<ViewMessages.SelectSession>>()
							.Subscribe(msg =>
							{
								if (_useEventMessages)
								{
									ReportInfo reportInfo = GetReportInfoFromRecordInfo(msg.RecordInfo);
									AddReportRecord(reportInfo);
								}
							});
		}

		private ReportInfo GetReportInfoFromRecordInfo(IFileRecordInfo recordInfo)
		{
			var session = RecordManager.LoadData(recordInfo.FullPath);

			double GeMetricValue(IList<double> sequence, EMetric metric) =>
					_frametimeStatisticProvider.GetFpsMetricValue(sequence, metric);

			var max = GeMetricValue(session.FrameTimes, EMetric.Max);
			var p99_quantile = GeMetricValue(session.FrameTimes, EMetric.P99);
			var p95_quantile = GeMetricValue(session.FrameTimes, EMetric.P95);
			var average = GeMetricValue(session.FrameTimes, EMetric.Average);
			var p0dot1_quantile = GeMetricValue(session.FrameTimes, EMetric.P0dot1);
			var p0dot2_quantile = GeMetricValue(session.FrameTimes, EMetric.P0dot2);
			var p1_quantile = GeMetricValue(session.FrameTimes, EMetric.P1);
			var p5_quantile = GeMetricValue(session.FrameTimes, EMetric.P5);
			var p1_averageLow = GeMetricValue(session.FrameTimes, EMetric.OnePercentLow);
			var p0dot1_averageLow = GeMetricValue(session.FrameTimes, EMetric.ZerodotOnePercentLow);
			var min = GeMetricValue(session.FrameTimes, EMetric.Min);
			var adaptiveStandardDeviation = GeMetricValue(session.FrameTimes, EMetric.AdaptiveStd);

			var reportInfo = new ReportInfo()
			{
				Game = recordInfo.GameName,
				Date = recordInfo.CreationDate,
				Time = recordInfo.CreationTime,
				NumberOfSamples = session.FrameTimes.Count,
				RecordTime = Math.Round(session.LastFrameTime, 2).ToString(CultureInfo.InvariantCulture),
				Cpu = recordInfo.ProcessorName == null ? "" : recordInfo.ProcessorName.Trim(new Char[] { ' ', '"' }),
				GraphicCard = recordInfo.GraphicCardName == null ? "" : recordInfo.GraphicCardName.Trim(new Char[] { ' ', '"' }),
				MaxFps = max,
				NinetyNinePercentQuantileFps = p99_quantile,
				NinetyFivePercentQuantileFps = p95_quantile,
				AverageFps = average,
				FivePercentQuantileFps = p5_quantile,
				OnePercentQuantileFps = p1_quantile,
				OnePercentLowAverageFps = p1_averageLow,
                ZeroDotTwoPercentQuantileFps = p0dot2_quantile,
                ZeroDotOnePercentQuantileFps = p0dot1_quantile,
				ZeroDotOnePercentLowAverageFps = p0dot1_averageLow,
				MinFps = min,
				AdaptiveSTDFps = adaptiveStandardDeviation,
				CustomComment = recordInfo.Comment
			};

			return reportInfo;
		}

		private void AddReportRecord(ReportInfo reportInfo)
		{
			ReportInfoCollecion.Add(reportInfo);
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			_useEventMessages = false;
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			_useEventMessages = true;
		}

		void IDropTarget.DragOver(IDropInfo dropInfo)
		{
			if (dropInfo != null)
			{
				dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
				dropInfo.Effects = DragDropEffects.Move;
			}
		}

		void IDropTarget.Drop(IDropInfo dropInfo)
		{
			if (dropInfo != null)
			{
				if (dropInfo.VisualTarget is FrameworkElement frameworkElement)
				{
					if (frameworkElement.Name == "ReportDataGrid")
					{
						if (dropInfo.Data is IFileRecordInfo recordInfo)
						{
							ReportInfo reportInfo = GetReportInfoFromRecordInfo(recordInfo);
							AddReportRecord(reportInfo);
						}
					}
					else if (frameworkElement.Name == "RemoveRecordItemControl")
					{
						if (dropInfo.Data is ReportInfo reportInfo)
						{
							ReportInfoCollecion.Remove(reportInfo);
						}

						if (dropInfo.Data is IEnumerable<ReportInfo> reportInfos)
						{
							reportInfos.ForEach(info => ReportInfoCollecion.Remove(info));
						}
					}
				}
			}
		}
	}
}