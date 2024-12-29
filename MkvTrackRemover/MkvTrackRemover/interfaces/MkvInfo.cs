using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MkvTrackRemover.interfaces
{
    internal class IAttachment
    {
        public string content_type { get; set; }
        public string description { get; set; }
        public string file_name { get; set; }
        public int id { get; set; }
        public IProperties properties { get; set; }
        public int size { get; set; }
    }

    internal class IContainer
    {
        public IProperties properties { get; set; }
        public bool recognized { get; set; }
        public bool supported { get; set; }
        public string type { get; set; }
    }

    internal class IGlobalTag
    {
        public int num_entries { get; set; }
    }

    internal class IProperties
    {
        public object uid { get; set; }
        public int container_type { get; set; }
        public DateTime date_local { get; set; }
        public DateTime date_utc { get; set; }
        public long duration { get; set; }
        public bool is_providing_timestamps { get; set; }
        public string muxing_application { get; set; }
        public string segment_uid { get; set; }
        public string writing_application { get; set; }
        public string chroma_siting { get; set; }
        public string codec_id { get; set; }
        public string codec_private_data { get; set; }
        public int codec_private_length { get; set; }
        public int default_duration { get; set; }
        public bool default_track { get; set; }
        public string display_dimensions { get; set; }
        public int display_unit { get; set; }
        public bool enabled_track { get; set; }
        public bool forced_track { get; set; }
        public string language { get; set; }
        public string language_ietf { get; set; }
        public long minimum_timestamp { get; set; }
        public int num_index_entries { get; set; }
        public int number { get; set; }
        public string packetizer { get; set; }
        public string pixel_dimensions { get; set; }
        public string tag_variant_bitrate { get; set; }
        public int? audio_bits_per_sample { get; set; }
        public int? audio_channels { get; set; }
        public int? audio_sampling_frequency { get; set; }
        public string encoding { get; set; }
        public bool? text_subtitles { get; set; }
    }

    internal class IMkvInfo
    {
        public List<IAttachment> attachments { get; set; }
        public List<object> chapters { get; set; }
        public IContainer container { get; set; }
        public List<object> errors { get; set; }
        public string file_name { get; set; }
        public List<IGlobalTag> global_tags { get; set; }
        public int identification_format_version { get; set; }
        public List<ITrackTag> track_tags { get; set; }
        public List<ITrack> tracks { get; set; }
        public List<object> warnings { get; set; }
    }

    internal class ELanguages
    {
        internal const string Japanese = "jpn";
        internal const string German = "ger";
        internal const string English = "eng";
        internal const string Undetermined = "und";
    }

    internal class ETrackType
    {
        internal const string Video = "video";
        internal const string Audio = "audio";
        internal const string Subtitles = "subtitles";
    }

    internal class ITrack
    {
        public string codec { get; set; }
        public int id { get; set; }
        public IProperties properties { get; set; }
        public string type { get; set; }
    }

    internal class ITrackTag
    {
        public int num_entries { get; set; }
        public int track_id { get; set; }
    }



    //internal class MkvAttachmentProperty
    //{
    //    int uid { get; set; }
    //}

    //internal class MkvAttachment
    //{
    //    string content_type { get; set; }
    //    string description { get; set; }
    //    string file_name { get; set; }
    //    int id { get; set; }
    //    MkvAttachmentProperty properties { get; set; }
    //    int size { get; set; }
    //}

    //internal class MkvChapter
    //{

    //}

    //internal class MkvContainerProperties
    //{
    //    int container_type { get; set; }
    //    string date_local { get; set; }
    //    string date_utc { get; set; }
    //    int duration { get; set; }
    //    bool is_providing_timestamps { get; set; }
    //    string muxing_application { get; set; }
    //    string segment_uid { get; set; }
    //    string writing_application { get; set; }
    //}

    //internal class MkvContainer
    //{
    //    MkvContainerProperties properties { get; set; }
    //    bool recognized { get; set; }
    //    bool supported { get; set; }
    //    string type { get; set; }
    //}

    //internal class MkvError
    //{

    //}

    //internal class MkvGlobalTag
    //{
    //    int num_entries { get; set; }
    //}

    //internal class MkvTrackTag
    //{
    //    int num_entries { get; set; }
    //    int track_id { get; set; }
    //}

    //internal class MkvTrackProperties
    //{
    //    string chroma_siting { get; set; }
    //    string codec_id { get; set; }
    //    string codec_private_data { get; set; }
    //    int codec_private_length { get; set; }
    //    int default_duration { get; set; }
    //    bool default_track { get; set; }

    //}

    //internal class MkvTrack
    //{
    //    string codec { get; set; }
    //    int id { get; set; }
    //    MkvTrackProperties[] properties { get; set; }
    //}

    //internal class MkvInfo
    //{
    //    MkvAttachment[] attachments { get; set; }
    //    MkvChapter chapters { get; set; }
    //    MkvContainer container { get; set; }
    //    MkvError[] errors { get; set; }
    //    string file_name { get; set; }
    //    MkvGlobalTag[] global_tags { get; set; }
    //    int identification_format_version { get; set; }
    //    MkvTrackTag[] track_tags { get; set; }
    //    MkvTrack[] tracks { get; set; }
    //}
}
