using QTrans.Contract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTrans.Common
{
    public abstract class ApplicationManager
    {
        protected ConcurrentDictionary<int, IProcessor> activeProcessor;

        public ApplicationManager()
        {
            this.activeProcessor = new ConcurrentDictionary<int, IProcessor>();
        }

        /// <summary>
        /// Stop all application processes.
        /// </summary>
        protected void StopProcesses()
        {
            if (this.activeProcessor != null && this.activeProcessor.Count > 0)
            {
                foreach (var keyValuePair in this.activeProcessor)
                {
                    IProcessor processor = null;
                    if (this.activeProcessor.TryRemove(keyValuePair.Key, out processor))
                    {
                        processor.StopOperation();
                        processor.Dispose();
                        processor = null;
                    }
                }

                this.activeProcessor.Clear();
            }
        }

        /// <summary>
        /// Stop application process by application processor id.
        /// </summary>
        /// <param name="id">Identity of processor</param>
        protected bool StopProcesses(int id)
        {
            var flg = false;
            if (this.activeProcessor != null && this.activeProcessor.Count > 0)
            {
                IProcessor processor = null;
                if (this.activeProcessor.TryRemove(id, out processor))
                {
                    flg = true;
                    processor.StopOperation();
                    processor.Dispose();
                    processor = null;
                }
            }

            return flg;
        }

        /// <summary>
        /// Start processor.
        /// </summary>
        /// <param name="data"></param>
        protected abstract void StartProcessor(IConfiguration data);

        /// <summary>
        /// Start Processor by Gateway ID
        /// </summary>
        /// <param name="gatewayId">Identity of Gateway</param>
        protected void AddProcessor(int ProcessId, IProcessor processor, out string logMessage)
        {
            logMessage = string.Empty;
            if (this.activeProcessor.TryAdd(ProcessId, processor))
            {
                if (this.activeProcessor[ProcessId].StartOperation())
                {
                    logMessage = string.Format("Processor is started", ProcessId);
                }
                else
                {
                    logMessage = string.Format("ProcessId {0} => Processor is not able to started", ProcessId);
                }
            }
            else
            {
                logMessage = string.Format("ProcessId {0} => Processor is not add in active Biller", ProcessId);
            }
        }

    }
}
