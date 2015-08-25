// -----------------------------------------------------------------------
// <copyright file="NpoiNewFormatAlignmentService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Service.Core.Service.Implement.Npoi
{
    using Fanex.Service.Core.Model.NpoiBase;
    using Fanex.Service.Core.Service.Interface.Npoi;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NpoiNewFormatAlignmentService : INpoiAlignmentService
    {
        private AlignmentInformation _leftAlignment;

        private AlignmentInformation _rightAlignment;

        private AlignmentInformation _centerAlignment;

        public NpoiNewFormatAlignmentService()
        {
            CreateLeftAlignment();
            CreateRightAlignment();
            CreateCenterAlignment();
        }

        public AlignmentInformation LeftAlignment
        {
            get
            {
                return _leftAlignment;
            }
        }

        public AlignmentInformation RightAlignment
        {
            get
            {
                return _rightAlignment;
            }
        }

        public AlignmentInformation CenterAlignment
        {
            get
            {
                return _centerAlignment;
            }
        }

        private void CreateLeftAlignment()
        {
            _leftAlignment = new AlignmentInformation
            {
                HorizontalAlignment = (int)HorizontalAlignment.Left,
                VerticalAlignment = (int)VerticalAlignment.Center
            };
        }

        private void CreateRightAlignment()
        {
            _rightAlignment = new AlignmentInformation
            {
                HorizontalAlignment = (int)HorizontalAlignment.Right,
                VerticalAlignment = (int)VerticalAlignment.Center
            };
        }

        private void CreateCenterAlignment()
        {
            _centerAlignment = new AlignmentInformation
            {
                HorizontalAlignment = (int)HorizontalAlignment.Center,
                VerticalAlignment = (int)VerticalAlignment.Center
            };
        }
    }
}